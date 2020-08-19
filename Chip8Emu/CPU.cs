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
            sp = 0;
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
            
            switch (op & 0xF000)
            {
                case 0x0000:
                    switch(op & 0xFF){
                        case 0x00E0: vram.clearScreen(); break;                     // cls
                        case 0x00EE: pc = stack[sp]; sp -= 1; break;                // ret
                    } break;
                case 0x1000: pc = addr; break;                              // jump (addr)
                case 0x2000:                                                // call (addr)
                    if(sp > 0) sp += 1;
                    stack[sp] = pc;
                    pc = addr;
                    break;
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
                case 0xB000: pc = (ushort) (addr + v[0]); break;
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
                        case 0x0A: v[x] = keypad.waitForKey(); break;
                        case 0x15: dt = v[x]; break;
                        case 0x18: st = v[x]; break;
                        case 0x1E: _i += v[x]; break;
                        case 0x29: ldf(x); break;
                        case 0x33: ldb(x); break;
                        case 0x55: for (var i = 0; i < x; i++) ram.writeByte((ushort) (_i + i), v[i]); break;
                        case 0x65: for (var i = 0; i < x; i++) v[i] = ram.readByte((ushort) (_i + i)); break;
                    } break;
            }
            pc += 2;
        }

        // Opcodes

        private void cls()
        {
            vram.clearScreen();
        }

        private void ret()
        {
            pc = stack[sp];
            sp -= 1;
        }

        private void jp(ushort addr)
        {
            pc = addr;
        }

        private void call(ushort addr)
        {
            if(sp > 0) sp += 1;
            stack[sp] = pc;
            pc = addr;
        }

        private void se(byte x, byte k)
        {
            if (v[x] == k) pc += 2;
        }

        private void sne(byte x, byte k)
        {
            if (v[x] != k) pc += 2;
        }

        private void sevv(byte x, byte y)
        {
            if (v[x] == v[y]) pc += 2;
        }

        private void ld(byte x, byte k)
        {
            v[x] = k;
        }

        private void add(byte x, byte k)
        {
            v[x] += k;
        }

        private void ldvv(byte x, byte y)
        {
            v[x] = v[y];
        }

        private void or(byte x, byte y)
        {
            v[x] |= v[y];
        }

        private void and(byte x, byte y)
        {
            v[x] &= v[y];
        }

        private void xor(byte x, byte y)
        {
            v[x] ^= v[y];
        }

        private void addvv(byte x, byte y)
        {
            v[0xF] = (byte)((v[x] + v[y] > 255) ? 1 : 0); v[x] += v[y];
        }

        private void sub(byte x, byte y)
        {
            
            v[0xf] = (byte)((v[x] > v[y]) ? 1 : 0); v[x] -= v[y];
        }

        private void shr(byte x)
        {
           
            v[0xf] = (byte)(((v[x] & 1) == 1) ? 1 : 0); v[x] >>= 1;
        }

        private void subn(byte x, byte y)
        {
            
            v[0xf] = (byte)((v[y] > v[x]) ? 1 : 0); v[x] = (byte)(v[y] - v[x]);
        }

        private void shl(byte x)
        {
            
            v[0xf] = (byte)(((v[x] & 128) > 0) ? 1 : 0); v[x] <<= 1;
        }

        private void snevv(byte x, byte y)
        {
            if (v[x] != v[y]) pc += 2;
        }

        private void ldi(ushort addr)
        {
            
        }

        private void jpv0(ushort addr)
        {
            pc = (ushort) (addr + v[0]);
        }

        private void rnd(byte x, byte k)
        {
            v[x] = (byte) (_random.Next(256) & k);
        }

        private void drw(byte x, byte y, byte n)
        {
            var ret = false;
            for (var i = 0; i < n; i++) ret = vram.drawSprite(v[x], v[y], ram.readBytes(_i, n));
            if (ret) v[0xf] = 1;
        }

        private void skp(byte x)
        {
            if (keypad.getKeyState(v[x])) pc += 2;
        }

        private void sknp(byte x)
        {
            if (!keypad.getKeyState(v[x])) pc += 2;
        }

        private void lddt(byte x)
        {
            v[x] = dt;
        }

        private void ldk(byte x)
        {
            v[x] = keypad.waitForKey();
        }

        private void lddtv(byte x)
        {
            dt = v[x];
        }

        private void ldst(byte x)
        {
            st = v[x];
        }

        private void addi(byte x)
        {
            _i += v[x];
        }

        private void ldf(byte x)
        {
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

        private void ldiv(byte x)
        {
            for (var i = 0; i < x; i++) ram.writeByte((ushort) (_i + i), v[i]);
        }

        private void ldvi(byte x)
        {
            for (var i = 0; i < x; i++) v[i] = ram.readByte((ushort) (_i + i));
        }
    }
}