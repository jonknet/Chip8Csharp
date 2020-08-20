using System;

namespace Chip8Emu
{
    public class CPU
    {
        private readonly ushort[] stack;
        private readonly byte[] v;
        private ushort _i;
        
        private readonly Random _random;
        private readonly IMemory ram;
        private readonly IVRAM vram;
        private readonly Keypad keypad;

        private byte dt;
        private ushort pc;
        private int sp;
        private byte st;

        public CPU(IMemory ram, IVRAM vram, Keypad keypad)
        {
            v = new byte[16];
            _i = 0;
            pc = 0x200;
            sp = -1;
            stack = new ushort[16];

            this.ram = ram;
            this.vram = vram;
            this.keypad = keypad;

            _random = new Random();
        }

        public void executeNext()
        {
            var op = ram.readWord(pc);
            var x = (byte) ((op & 0x0F00) >> 8);
            var y = (byte) ((op & 0x00F0) >> 4);
            var k = (byte) (op & 0xFF);
            var n = (byte) (op & 0xF);
            var addr = (ushort) (op & 0xFFF);
            
            processTimers();
            
            Console.WriteLine("==");
            Console.WriteLine("op: {0:X}, x: {1:X}, y: {2:X}, k: {3:X}, n: {4:X}, addr: {5:X}",op,x,y,k,n,addr);
            Console.Write("V: "); foreach(byte b in v) { Console.Write("{0:X} ",b);} Console.WriteLine();
            Console.Write("Stack: "); foreach(ushort u in stack) {Console.Write("{0:X} ",u);} Console.WriteLine();
            Console.WriteLine("sp: {0:X}, pc: {1:X}, _i: {2:X}, dt: {3}, st: {4}",sp,pc,_i,dt,st);
            
            
            
            switch (op & 0xF000)
            {
                case 0x0000:
                    switch(op & 0xFF){
                        case 0x00E0: vram.clearScreen(); break;                     
                        case 0x00EE: pc = stack[sp]; sp -= 1; break;          
                    } break;
                case 0x1000: pc = addr; return;                              
                case 0x2000:                                                
                    sp += 1;
                    stack[sp] = pc;
                    pc = addr;
                    return;
                case 0x3000: if (v[x] == k) pc += 2; break;
                case 0x4000: if (v[x] != k) pc += 2; break;
                case 0x5000: if (v[x] == v[y]) pc += 2; break;
                case 0x6000: v[x] = k; break;
                case 0x7000: v[x] += k; break;
                case 0x8000:
                    switch(op & 0xF){
                        case 0x0001: v[x] |= v[y]; break;
                        case 0x0002: v[x] &= v[y]; break;
                        case 0x0003: v[x] ^= v[y]; break;
                        case 0x0004: v[0xF] = (byte)((v[x] + v[y] > 255) ? 1 : 0); v[x] += v[y]; break;
                        case 0x0005: v[0xf] = (byte)((v[x] > v[y]) ? 1 : 0); v[x] -= v[y]; break;
                        case 0x0006: v[0xf] = (byte)(((v[x] & 1) == 1) ? 1 : 0); v[x] >>= 1; break;
                        case 0x0007: v[0xf] = (byte)((v[y] > v[x]) ? 1 : 0); v[x] = (byte)(v[y] - v[x]); break;
                        case 0x000E: v[0xf] = (byte)(((v[x] & 128) > 0) ? 1 : 0); v[x] <<= 1; break;
                    } break;
                case 0x9000: if (v[x] != v[y]) pc += 2; break;
                case 0xA000: _i = addr; break;
                case 0xB000: pc = (ushort) (addr + v[0]); return;
                case 0xC000: v[x] = (byte) (_random.Next(256) & k); break;
                case 0xD000: drw(x, y, n); break;
                case 0xE000:
                    switch(op & 0xFF){
                        case 0x009E: if (keypad.getKeyState(v[x])) pc += 2; break;
                        case 0x00A1: if (!keypad.getKeyState(v[x])) pc += 2; break;
                    } break;
                case 0xF000:
                    switch (op & 0xFF){
                        case 0x07: v[x] = dt; break;
                        case 0x0A: ldvk(x); break;
                        case 0x15: dt = v[x]; break;
                        case 0x18: st = v[x]; break;
                        case 0x1E: _i += v[x]; break;
                        case 0x29: _i = Memory.HEX_CHAR_ADDR[v[x] % 16]; break;
                        case 0x33: ldb(x); break;
                        case 0x55: for (var i = 0; i <= x; i++) ram.writeByte((ushort) (_i + i), v[i]); break;
                        case 0x65: for (var i = 0; i <= x; i++) v[i] = ram.readByte((ushort) (_i + i)); break;
                    } break;
            }
            pc += 2;
        }

        // Opcodes
        
        private void drw(byte x, byte y, byte n)
        {
            var ret = false;
            for (var i = 0; i < n; i++) ret = vram.drawSprite(v[x], v[y], ram.readBytes(_i, n));
            if (ret) v[0xf] = 1;
        }
        
        private void ldvk(byte x)
        {
            var k = keypad.getCurrentPressedKey();
            if(k == 0xFF)
            {
                pc -= 2;
            } else
            {
                v[x] = k;
            }
        }
        
        private void ldb(byte x)
        {
            var hundreds = (byte) (v[x] / 100);
            var tens = (byte)((x % 100) / 10);
            var units = (byte)(x % 10);

            ram.writeByte(_i, hundreds);
            ram.writeByte((ushort) (_i + 1), tens);
            ram.writeByte((ushort) (_i + 2), units);
        }

        private void processTimers()
        {
            if(dt > 0)
            {
                dt -= 1;
            }
            
            if(st > 0)
            {
                st -= 1;
            }
        }
    }
}