namespace BestHTTP.Decompression.Zlib
{
    using System;
    using System.Runtime.CompilerServices;

    internal sealed class DeflateManager
    {
        private static readonly int MEM_LEVEL_MAX = 9;
        private static readonly int MEM_LEVEL_DEFAULT = 8;
        private CompressFunc DeflateFunction;
        private static readonly string[] _ErrorMessage = new string[] { "need dictionary", "stream end", string.Empty, "file error", "stream error", "data error", "insufficient memory", "buffer error", "incompatible version", string.Empty };
        private static readonly int PRESET_DICT = 0x20;
        private static readonly int INIT_STATE = 0x2a;
        private static readonly int BUSY_STATE = 0x71;
        private static readonly int FINISH_STATE = 0x29a;
        private static readonly int Z_DEFLATED = 8;
        private static readonly int STORED_BLOCK = 0;
        private static readonly int STATIC_TREES = 1;
        private static readonly int DYN_TREES = 2;
        private static readonly int Z_BINARY = 0;
        private static readonly int Z_ASCII = 1;
        private static readonly int Z_UNKNOWN = 2;
        private static readonly int Buf_size = 0x10;
        private static readonly int MIN_MATCH = 3;
        private static readonly int MAX_MATCH = 0x102;
        private static readonly int MIN_LOOKAHEAD = ((MAX_MATCH + MIN_MATCH) + 1);
        private static readonly int HEAP_SIZE = ((2 * InternalConstants.L_CODES) + 1);
        private static readonly int END_BLOCK = 0x100;
        internal ZlibCodec _codec;
        internal int status;
        internal byte[] pending;
        internal int nextPending;
        internal int pendingCount;
        internal sbyte data_type;
        internal int last_flush;
        internal int w_size;
        internal int w_bits;
        internal int w_mask;
        internal byte[] window;
        internal int window_size;
        internal short[] prev;
        internal short[] head;
        internal int ins_h;
        internal int hash_size;
        internal int hash_bits;
        internal int hash_mask;
        internal int hash_shift;
        internal int block_start;
        private Config config;
        internal int match_length;
        internal int prev_match;
        internal int match_available;
        internal int strstart;
        internal int match_start;
        internal int lookahead;
        internal int prev_length;
        internal CompressionLevel compressionLevel;
        internal CompressionStrategy compressionStrategy;
        internal short[] dyn_ltree = new short[HEAP_SIZE * 2];
        internal short[] dyn_dtree = new short[((2 * InternalConstants.D_CODES) + 1) * 2];
        internal short[] bl_tree = new short[((2 * InternalConstants.BL_CODES) + 1) * 2];
        internal ZTree treeLiterals = new ZTree();
        internal ZTree treeDistances = new ZTree();
        internal ZTree treeBitLengths = new ZTree();
        internal short[] bl_count = new short[InternalConstants.MAX_BITS + 1];
        internal int[] heap = new int[(2 * InternalConstants.L_CODES) + 1];
        internal int heap_len;
        internal int heap_max;
        internal sbyte[] depth = new sbyte[(2 * InternalConstants.L_CODES) + 1];
        internal int _lengthOffset;
        internal int lit_bufsize;
        internal int last_lit;
        internal int _distanceOffset;
        internal int opt_len;
        internal int static_len;
        internal int matches;
        internal int last_eob_len;
        internal short bi_buf;
        internal int bi_valid;
        private bool Rfc1950BytesEmitted;
        private bool _WantRfc1950HeaderBytes = true;

        internal DeflateManager()
        {
        }

        private void _fillWindow()
        {
            int num;
            int num4;
        Label_0000:
            num4 = (this.window_size - this.lookahead) - this.strstart;
            if (((num4 == 0) && (this.strstart == 0)) && (this.lookahead == 0))
            {
                num4 = this.w_size;
            }
            else if (num4 == -1)
            {
                num4--;
            }
            else if (this.strstart >= ((this.w_size + this.w_size) - MIN_LOOKAHEAD))
            {
                int num2;
                Array.Copy(this.window, this.w_size, this.window, 0, this.w_size);
                this.match_start -= this.w_size;
                this.strstart -= this.w_size;
                this.block_start -= this.w_size;
                num = this.hash_size;
                int index = num;
                do
                {
                    num2 = this.head[--index] & 0xffff;
                    this.head[index] = (num2 < this.w_size) ? ((short) 0) : ((short) (num2 - this.w_size));
                }
                while (--num != 0);
                num = this.w_size;
                index = num;
                do
                {
                    num2 = this.prev[--index] & 0xffff;
                    this.prev[index] = (num2 < this.w_size) ? ((short) 0) : ((short) (num2 - this.w_size));
                }
                while (--num != 0);
                num4 += this.w_size;
            }
            if (this._codec.AvailableBytesIn != 0)
            {
                num = this._codec.read_buf(this.window, this.strstart + this.lookahead, num4);
                this.lookahead += num;
                if (this.lookahead >= MIN_MATCH)
                {
                    this.ins_h = this.window[this.strstart] & 0xff;
                    this.ins_h = ((this.ins_h << this.hash_shift) ^ (this.window[this.strstart + 1] & 0xff)) & this.hash_mask;
                }
                if ((this.lookahead < MIN_LOOKAHEAD) && (this._codec.AvailableBytesIn != 0))
                {
                    goto Label_0000;
                }
            }
        }

        internal void _InitializeBlocks()
        {
            for (int i = 0; i < InternalConstants.L_CODES; i++)
            {
                this.dyn_ltree[i * 2] = 0;
            }
            for (int j = 0; j < InternalConstants.D_CODES; j++)
            {
                this.dyn_dtree[j * 2] = 0;
            }
            for (int k = 0; k < InternalConstants.BL_CODES; k++)
            {
                this.bl_tree[k * 2] = 0;
            }
            this.dyn_ltree[END_BLOCK * 2] = 1;
            this.opt_len = this.static_len = 0;
            this.last_lit = this.matches = 0;
        }

        private void _InitializeLazyMatch()
        {
            this.window_size = 2 * this.w_size;
            Array.Clear(this.head, 0, this.hash_size);
            this.config = Config.Lookup(this.compressionLevel);
            this.SetDeflater();
            this.strstart = 0;
            this.block_start = 0;
            this.lookahead = 0;
            this.match_length = this.prev_length = MIN_MATCH - 1;
            this.match_available = 0;
            this.ins_h = 0;
        }

        private void _InitializeTreeData()
        {
            this.treeLiterals.dyn_tree = this.dyn_ltree;
            this.treeLiterals.staticTree = StaticTree.Literals;
            this.treeDistances.dyn_tree = this.dyn_dtree;
            this.treeDistances.staticTree = StaticTree.Distances;
            this.treeBitLengths.dyn_tree = this.bl_tree;
            this.treeBitLengths.staticTree = StaticTree.BitLengths;
            this.bi_buf = 0;
            this.bi_valid = 0;
            this.last_eob_len = 8;
            this._InitializeBlocks();
        }

        internal static bool _IsSmaller(short[] tree, int n, int m, sbyte[] depth)
        {
            short num = tree[n * 2];
            short num2 = tree[m * 2];
            return ((num < num2) || ((num == num2) && (depth[n] <= depth[m])));
        }

        internal void _tr_align()
        {
            this.send_bits(STATIC_TREES << 1, 3);
            this.send_code(END_BLOCK, StaticTree.lengthAndLiteralsTreeCodes);
            this.bi_flush();
            if ((((1 + this.last_eob_len) + 10) - this.bi_valid) < 9)
            {
                this.send_bits(STATIC_TREES << 1, 3);
                this.send_code(END_BLOCK, StaticTree.lengthAndLiteralsTreeCodes);
                this.bi_flush();
            }
            this.last_eob_len = 7;
        }

        internal void _tr_flush_block(int buf, int stored_len, bool eof)
        {
            int num;
            int num2;
            int num3 = 0;
            if (this.compressionLevel > CompressionLevel.None)
            {
                if (this.data_type == Z_UNKNOWN)
                {
                    this.set_data_type();
                }
                this.treeLiterals.build_tree(this);
                this.treeDistances.build_tree(this);
                num3 = this.build_bl_tree();
                num = ((this.opt_len + 3) + 7) >> 3;
                num2 = ((this.static_len + 3) + 7) >> 3;
                if (num2 <= num)
                {
                    num = num2;
                }
            }
            else
            {
                num = num2 = stored_len + 5;
            }
            if (((stored_len + 4) <= num) && (buf != -1))
            {
                this._tr_stored_block(buf, stored_len, eof);
            }
            else if (num2 == num)
            {
                this.send_bits((STATIC_TREES << 1) + (!eof ? 0 : 1), 3);
                this.send_compressed_block(StaticTree.lengthAndLiteralsTreeCodes, StaticTree.distTreeCodes);
            }
            else
            {
                this.send_bits((DYN_TREES << 1) + (!eof ? 0 : 1), 3);
                this.send_all_trees(this.treeLiterals.max_code + 1, this.treeDistances.max_code + 1, num3 + 1);
                this.send_compressed_block(this.dyn_ltree, this.dyn_dtree);
            }
            this._InitializeBlocks();
            if (eof)
            {
                this.bi_windup();
            }
        }

        internal void _tr_stored_block(int buf, int stored_len, bool eof)
        {
            this.send_bits((STORED_BLOCK << 1) + (!eof ? 0 : 1), 3);
            this.copy_block(buf, stored_len, true);
        }

        internal bool _tr_tally(int dist, int lc)
        {
            this.pending[this._distanceOffset + (this.last_lit * 2)] = (byte) (dist >> 8);
            this.pending[(this._distanceOffset + (this.last_lit * 2)) + 1] = (byte) dist;
            this.pending[this._lengthOffset + this.last_lit] = (byte) lc;
            this.last_lit++;
            if (dist == 0)
            {
                this.dyn_ltree[lc * 2] = (short) (this.dyn_ltree[lc * 2] + 1);
            }
            else
            {
                this.matches++;
                dist--;
                this.dyn_ltree[((ZTree.LengthCode[lc] + InternalConstants.LITERALS) + 1) * 2] = (short) (this.dyn_ltree[((ZTree.LengthCode[lc] + InternalConstants.LITERALS) + 1) * 2] + 1);
                this.dyn_dtree[ZTree.DistanceCode(dist) * 2] = (short) (this.dyn_dtree[ZTree.DistanceCode(dist) * 2] + 1);
            }
            if (((this.last_lit & 0x1fff) == 0) && (this.compressionLevel > CompressionLevel.Level2))
            {
                int num = this.last_lit << 3;
                int num2 = this.strstart - this.block_start;
                for (int i = 0; i < InternalConstants.D_CODES; i++)
                {
                    num += (int) (this.dyn_dtree[i * 2] * (5L + ZTree.ExtraDistanceBits[i]));
                }
                num = num >> 3;
                if ((this.matches < (this.last_lit / 2)) && (num < (num2 / 2)))
                {
                    return true;
                }
            }
            return ((this.last_lit == (this.lit_bufsize - 1)) || (this.last_lit == this.lit_bufsize));
        }

        internal void bi_flush()
        {
            if (this.bi_valid == 0x10)
            {
                this.pending[this.pendingCount++] = (byte) this.bi_buf;
                this.pending[this.pendingCount++] = (byte) (this.bi_buf >> 8);
                this.bi_buf = 0;
                this.bi_valid = 0;
            }
            else if (this.bi_valid >= 8)
            {
                this.pending[this.pendingCount++] = (byte) this.bi_buf;
                this.bi_buf = (short) (this.bi_buf >> 8);
                this.bi_valid -= 8;
            }
        }

        internal void bi_windup()
        {
            if (this.bi_valid > 8)
            {
                this.pending[this.pendingCount++] = (byte) this.bi_buf;
                this.pending[this.pendingCount++] = (byte) (this.bi_buf >> 8);
            }
            else if (this.bi_valid > 0)
            {
                this.pending[this.pendingCount++] = (byte) this.bi_buf;
            }
            this.bi_buf = 0;
            this.bi_valid = 0;
        }

        internal int build_bl_tree()
        {
            this.scan_tree(this.dyn_ltree, this.treeLiterals.max_code);
            this.scan_tree(this.dyn_dtree, this.treeDistances.max_code);
            this.treeBitLengths.build_tree(this);
            int index = InternalConstants.BL_CODES - 1;
            while (index >= 3)
            {
                if (this.bl_tree[(ZTree.bl_order[index] * 2) + 1] != 0)
                {
                    break;
                }
                index--;
            }
            this.opt_len += (((3 * (index + 1)) + 5) + 5) + 4;
            return index;
        }

        internal void copy_block(int buf, int len, bool header)
        {
            this.bi_windup();
            this.last_eob_len = 8;
            if (header)
            {
                this.pending[this.pendingCount++] = (byte) len;
                this.pending[this.pendingCount++] = (byte) (len >> 8);
                this.pending[this.pendingCount++] = (byte) ~len;
                this.pending[this.pendingCount++] = (byte) (~len >> 8);
            }
            this.put_bytes(this.window, buf, len);
        }

        internal int Deflate(FlushType flush)
        {
            if (((this._codec.OutputBuffer == null) || ((this._codec.InputBuffer == null) && (this._codec.AvailableBytesIn != 0))) || ((this.status == FINISH_STATE) && (flush != FlushType.Finish)))
            {
                this._codec.Message = _ErrorMessage[4];
                throw new ZlibException($"Something is fishy. [{this._codec.Message}]");
            }
            if (this._codec.AvailableBytesOut == 0)
            {
                this._codec.Message = _ErrorMessage[7];
                throw new ZlibException("OutputBuffer is full (AvailableBytesOut == 0)");
            }
            int num = this.last_flush;
            this.last_flush = (int) flush;
            if (this.status == INIT_STATE)
            {
                int num2 = (Z_DEFLATED + ((this.w_bits - 8) << 4)) << 8;
                int num3 = ((int) ((this.compressionLevel - 1) & 0xff)) >> 1;
                if (num3 > 3)
                {
                    num3 = 3;
                }
                num2 |= num3 << 6;
                if (this.strstart != 0)
                {
                    num2 |= PRESET_DICT;
                }
                num2 += 0x1f - (num2 % 0x1f);
                this.status = BUSY_STATE;
                this.pending[this.pendingCount++] = (byte) (num2 >> 8);
                this.pending[this.pendingCount++] = (byte) num2;
                if (this.strstart != 0)
                {
                    this.pending[this.pendingCount++] = (byte) ((this._codec._Adler32 & -16777216) >> 0x18);
                    this.pending[this.pendingCount++] = (byte) ((this._codec._Adler32 & 0xff0000) >> 0x10);
                    this.pending[this.pendingCount++] = (byte) ((this._codec._Adler32 & 0xff00) >> 8);
                    this.pending[this.pendingCount++] = (byte) (this._codec._Adler32 & 0xff);
                }
                this._codec._Adler32 = Adler.Adler32(0, null, 0, 0);
            }
            if (this.pendingCount != 0)
            {
                this._codec.flush_pending();
                if (this._codec.AvailableBytesOut == 0)
                {
                    this.last_flush = -1;
                    return 0;
                }
            }
            else if (((this._codec.AvailableBytesIn == 0) && (flush <= num)) && (flush != FlushType.Finish))
            {
                return 0;
            }
            if ((this.status == FINISH_STATE) && (this._codec.AvailableBytesIn != 0))
            {
                this._codec.Message = _ErrorMessage[7];
                throw new ZlibException("status == FINISH_STATE && _codec.AvailableBytesIn != 0");
            }
            if (((this._codec.AvailableBytesIn != 0) || (this.lookahead != 0)) || ((flush != FlushType.None) && (this.status != FINISH_STATE)))
            {
                BlockState state = this.DeflateFunction(flush);
                switch (state)
                {
                    case BlockState.FinishStarted:
                    case BlockState.FinishDone:
                        this.status = FINISH_STATE;
                        break;
                }
                if ((state == BlockState.NeedMore) || (state == BlockState.FinishStarted))
                {
                    if (this._codec.AvailableBytesOut == 0)
                    {
                        this.last_flush = -1;
                    }
                    return 0;
                }
                if (state == BlockState.BlockDone)
                {
                    if (flush == FlushType.Partial)
                    {
                        this._tr_align();
                    }
                    else
                    {
                        this._tr_stored_block(0, 0, false);
                        if (flush == FlushType.Full)
                        {
                            for (int i = 0; i < this.hash_size; i++)
                            {
                                this.head[i] = 0;
                            }
                        }
                    }
                    this._codec.flush_pending();
                    if (this._codec.AvailableBytesOut == 0)
                    {
                        this.last_flush = -1;
                        return 0;
                    }
                }
            }
            if (flush != FlushType.Finish)
            {
                return 0;
            }
            if (!this.WantRfc1950HeaderBytes || this.Rfc1950BytesEmitted)
            {
                return 1;
            }
            this.pending[this.pendingCount++] = (byte) ((this._codec._Adler32 & -16777216) >> 0x18);
            this.pending[this.pendingCount++] = (byte) ((this._codec._Adler32 & 0xff0000) >> 0x10);
            this.pending[this.pendingCount++] = (byte) ((this._codec._Adler32 & 0xff00) >> 8);
            this.pending[this.pendingCount++] = (byte) (this._codec._Adler32 & 0xff);
            this._codec.flush_pending();
            this.Rfc1950BytesEmitted = true;
            return ((this.pendingCount == 0) ? 1 : 0);
        }

        internal BlockState DeflateFast(FlushType flush)
        {
            bool flag;
            int num = 0;
        Label_0002:
            if (this.lookahead < MIN_LOOKAHEAD)
            {
                this._fillWindow();
                if ((this.lookahead < MIN_LOOKAHEAD) && (flush == FlushType.None))
                {
                    return BlockState.NeedMore;
                }
                if (this.lookahead == 0)
                {
                    this.flush_block_only(flush == FlushType.Finish);
                    if (this._codec.AvailableBytesOut != 0)
                    {
                        return ((flush != FlushType.Finish) ? BlockState.BlockDone : BlockState.FinishDone);
                    }
                    if (flush == FlushType.Finish)
                    {
                        return BlockState.FinishStarted;
                    }
                    return BlockState.NeedMore;
                }
            }
            if (this.lookahead >= MIN_MATCH)
            {
                this.ins_h = ((this.ins_h << this.hash_shift) ^ (this.window[this.strstart + (MIN_MATCH - 1)] & 0xff)) & this.hash_mask;
                num = this.head[this.ins_h] & 0xffff;
                this.prev[this.strstart & this.w_mask] = this.head[this.ins_h];
                this.head[this.ins_h] = (short) this.strstart;
            }
            if (((num != 0L) && (((this.strstart - num) & 0xffff) <= (this.w_size - MIN_LOOKAHEAD))) && (this.compressionStrategy != CompressionStrategy.HuffmanOnly))
            {
                this.match_length = this.longest_match(num);
            }
            if (this.match_length >= MIN_MATCH)
            {
                flag = this._tr_tally(this.strstart - this.match_start, this.match_length - MIN_MATCH);
                this.lookahead -= this.match_length;
                if ((this.match_length <= this.config.MaxLazy) && (this.lookahead >= MIN_MATCH))
                {
                    this.match_length--;
                    do
                    {
                        this.strstart++;
                        this.ins_h = ((this.ins_h << this.hash_shift) ^ (this.window[this.strstart + (MIN_MATCH - 1)] & 0xff)) & this.hash_mask;
                        num = this.head[this.ins_h] & 0xffff;
                        this.prev[this.strstart & this.w_mask] = this.head[this.ins_h];
                        this.head[this.ins_h] = (short) this.strstart;
                    }
                    while (--this.match_length != 0);
                    this.strstart++;
                }
                else
                {
                    this.strstart += this.match_length;
                    this.match_length = 0;
                    this.ins_h = this.window[this.strstart] & 0xff;
                    this.ins_h = ((this.ins_h << this.hash_shift) ^ (this.window[this.strstart + 1] & 0xff)) & this.hash_mask;
                }
            }
            else
            {
                flag = this._tr_tally(0, this.window[this.strstart] & 0xff);
                this.lookahead--;
                this.strstart++;
            }
            if (!flag)
            {
                goto Label_0002;
            }
            this.flush_block_only(false);
            if (this._codec.AvailableBytesOut != 0)
            {
                goto Label_0002;
            }
            return BlockState.NeedMore;
        }

        internal BlockState DeflateNone(FlushType flush)
        {
            int num = 0xffff;
            if (num > (this.pending.Length - 5))
            {
                num = this.pending.Length - 5;
            }
        Label_0021:
            if (this.lookahead <= 1)
            {
                this._fillWindow();
                if ((this.lookahead == 0) && (flush == FlushType.None))
                {
                    return BlockState.NeedMore;
                }
                if (this.lookahead == 0)
                {
                    this.flush_block_only(flush == FlushType.Finish);
                    if (this._codec.AvailableBytesOut == 0)
                    {
                        return ((flush != FlushType.Finish) ? BlockState.NeedMore : BlockState.FinishStarted);
                    }
                    return ((flush != FlushType.Finish) ? BlockState.BlockDone : BlockState.FinishDone);
                }
            }
            this.strstart += this.lookahead;
            this.lookahead = 0;
            int num2 = this.block_start + num;
            if ((this.strstart == 0) || (this.strstart >= num2))
            {
                this.lookahead = this.strstart - num2;
                this.strstart = num2;
                this.flush_block_only(false);
                if (this._codec.AvailableBytesOut == 0)
                {
                    return BlockState.NeedMore;
                }
            }
            if ((this.strstart - this.block_start) < (this.w_size - MIN_LOOKAHEAD))
            {
                goto Label_0021;
            }
            this.flush_block_only(false);
            if (this._codec.AvailableBytesOut != 0)
            {
                goto Label_0021;
            }
            return BlockState.NeedMore;
        }

        internal BlockState DeflateSlow(FlushType flush)
        {
            bool flag;
            int num = 0;
            while (true)
            {
                if (this.lookahead < MIN_LOOKAHEAD)
                {
                    this._fillWindow();
                    if ((this.lookahead < MIN_LOOKAHEAD) && (flush == FlushType.None))
                    {
                        return BlockState.NeedMore;
                    }
                    if (this.lookahead == 0)
                    {
                        break;
                    }
                }
                if (this.lookahead >= MIN_MATCH)
                {
                    this.ins_h = ((this.ins_h << this.hash_shift) ^ (this.window[this.strstart + (MIN_MATCH - 1)] & 0xff)) & this.hash_mask;
                    num = this.head[this.ins_h] & 0xffff;
                    this.prev[this.strstart & this.w_mask] = this.head[this.ins_h];
                    this.head[this.ins_h] = (short) this.strstart;
                }
                this.prev_length = this.match_length;
                this.prev_match = this.match_start;
                this.match_length = MIN_MATCH - 1;
                if (((num != 0) && (this.prev_length < this.config.MaxLazy)) && (((this.strstart - num) & 0xffff) <= (this.w_size - MIN_LOOKAHEAD)))
                {
                    if (this.compressionStrategy != CompressionStrategy.HuffmanOnly)
                    {
                        this.match_length = this.longest_match(num);
                    }
                    if ((this.match_length <= 5) && ((this.compressionStrategy == CompressionStrategy.Filtered) || ((this.match_length == MIN_MATCH) && ((this.strstart - this.match_start) > 0x1000))))
                    {
                        this.match_length = MIN_MATCH - 1;
                    }
                }
                if ((this.prev_length >= MIN_MATCH) && (this.match_length <= this.prev_length))
                {
                    int num2 = (this.strstart + this.lookahead) - MIN_MATCH;
                    flag = this._tr_tally((this.strstart - 1) - this.prev_match, this.prev_length - MIN_MATCH);
                    this.lookahead -= this.prev_length - 1;
                    this.prev_length -= 2;
                    do
                    {
                        if (++this.strstart <= num2)
                        {
                            this.ins_h = ((this.ins_h << this.hash_shift) ^ (this.window[this.strstart + (MIN_MATCH - 1)] & 0xff)) & this.hash_mask;
                            num = this.head[this.ins_h] & 0xffff;
                            this.prev[this.strstart & this.w_mask] = this.head[this.ins_h];
                            this.head[this.ins_h] = (short) this.strstart;
                        }
                    }
                    while (--this.prev_length != 0);
                    this.match_available = 0;
                    this.match_length = MIN_MATCH - 1;
                    this.strstart++;
                    if (flag)
                    {
                        this.flush_block_only(false);
                        if (this._codec.AvailableBytesOut == 0)
                        {
                            return BlockState.NeedMore;
                        }
                    }
                }
                else if (this.match_available != 0)
                {
                    if (this._tr_tally(0, this.window[this.strstart - 1] & 0xff))
                    {
                        this.flush_block_only(false);
                    }
                    this.strstart++;
                    this.lookahead--;
                    if (this._codec.AvailableBytesOut == 0)
                    {
                        return BlockState.NeedMore;
                    }
                }
                else
                {
                    this.match_available = 1;
                    this.strstart++;
                    this.lookahead--;
                }
            }
            if (this.match_available != 0)
            {
                flag = this._tr_tally(0, this.window[this.strstart - 1] & 0xff);
                this.match_available = 0;
            }
            this.flush_block_only(flush == FlushType.Finish);
            if (this._codec.AvailableBytesOut != 0)
            {
                return ((flush != FlushType.Finish) ? BlockState.BlockDone : BlockState.FinishDone);
            }
            if (flush == FlushType.Finish)
            {
                return BlockState.FinishStarted;
            }
            return BlockState.NeedMore;
        }

        internal int End()
        {
            if (((this.status != INIT_STATE) && (this.status != BUSY_STATE)) && (this.status != FINISH_STATE))
            {
                return -2;
            }
            this.pending = null;
            this.head = null;
            this.prev = null;
            this.window = null;
            return ((this.status != BUSY_STATE) ? 0 : -3);
        }

        internal void flush_block_only(bool eof)
        {
            this._tr_flush_block((this.block_start < 0) ? -1 : this.block_start, this.strstart - this.block_start, eof);
            this.block_start = this.strstart;
            this._codec.flush_pending();
        }

        internal int Initialize(ZlibCodec codec, CompressionLevel level) => 
            this.Initialize(codec, level, 15);

        internal int Initialize(ZlibCodec codec, CompressionLevel level, int bits) => 
            this.Initialize(codec, level, bits, MEM_LEVEL_DEFAULT, CompressionStrategy.Default);

        internal int Initialize(ZlibCodec codec, CompressionLevel level, int bits, CompressionStrategy compressionStrategy) => 
            this.Initialize(codec, level, bits, MEM_LEVEL_DEFAULT, compressionStrategy);

        internal int Initialize(ZlibCodec codec, CompressionLevel level, int windowBits, int memLevel, CompressionStrategy strategy)
        {
            this._codec = codec;
            this._codec.Message = null;
            if ((windowBits < 9) || (windowBits > 15))
            {
                throw new ZlibException("windowBits must be in the range 9..15.");
            }
            if ((memLevel < 1) || (memLevel > MEM_LEVEL_MAX))
            {
                throw new ZlibException($"memLevel must be in the range 1.. {MEM_LEVEL_MAX}");
            }
            this._codec.dstate = this;
            this.w_bits = windowBits;
            this.w_size = ((int) 1) << this.w_bits;
            this.w_mask = this.w_size - 1;
            this.hash_bits = memLevel + 7;
            this.hash_size = ((int) 1) << this.hash_bits;
            this.hash_mask = this.hash_size - 1;
            this.hash_shift = ((this.hash_bits + MIN_MATCH) - 1) / MIN_MATCH;
            this.window = new byte[this.w_size * 2];
            this.prev = new short[this.w_size];
            this.head = new short[this.hash_size];
            this.lit_bufsize = ((int) 1) << (memLevel + 6);
            this.pending = new byte[this.lit_bufsize * 4];
            this._distanceOffset = this.lit_bufsize;
            this._lengthOffset = 3 * this.lit_bufsize;
            this.compressionLevel = level;
            this.compressionStrategy = strategy;
            this.Reset();
            return 0;
        }

        internal int longest_match(int cur_match)
        {
            int maxChainLength = this.config.MaxChainLength;
            int strstart = this.strstart;
            int num5 = this.prev_length;
            int num6 = (this.strstart <= (this.w_size - MIN_LOOKAHEAD)) ? 0 : (this.strstart - (this.w_size - MIN_LOOKAHEAD));
            int niceLength = this.config.NiceLength;
            int num8 = this.w_mask;
            int num9 = this.strstart + MAX_MATCH;
            byte num10 = this.window[(strstart + num5) - 1];
            byte num11 = this.window[strstart + num5];
            if (this.prev_length >= this.config.GoodLength)
            {
                maxChainLength = maxChainLength >> 2;
            }
            if (niceLength > this.lookahead)
            {
                niceLength = this.lookahead;
            }
            do
            {
                int index = cur_match;
                if ((((this.window[index + num5] == num11) && (this.window[(index + num5) - 1] == num10)) && (this.window[index] == this.window[strstart])) && (this.window[++index] == this.window[strstart + 1]))
                {
                    strstart += 2;
                    index++;
                    while ((((this.window[++strstart] == this.window[++index]) && (this.window[++strstart] == this.window[++index])) && ((this.window[++strstart] == this.window[++index]) && (this.window[++strstart] == this.window[++index]))) && (((this.window[++strstart] == this.window[++index]) && (this.window[++strstart] == this.window[++index])) && (((this.window[++strstart] == this.window[++index]) && (this.window[++strstart] == this.window[++index])) && (strstart < num9))))
                    {
                    }
                    int num4 = MAX_MATCH - (num9 - strstart);
                    strstart = num9 - MAX_MATCH;
                    if (num4 > num5)
                    {
                        this.match_start = cur_match;
                        num5 = num4;
                        if (num4 >= niceLength)
                        {
                            break;
                        }
                        num10 = this.window[(strstart + num5) - 1];
                        num11 = this.window[strstart + num5];
                    }
                }
            }
            while (((cur_match = this.prev[cur_match & num8] & 0xffff) > num6) && (--maxChainLength != 0));
            if (num5 <= this.lookahead)
            {
                return num5;
            }
            return this.lookahead;
        }

        internal void pqdownheap(short[] tree, int k)
        {
            int n = this.heap[k];
            for (int i = k << 1; i <= this.heap_len; i = i << 1)
            {
                if ((i < this.heap_len) && _IsSmaller(tree, this.heap[i + 1], this.heap[i], this.depth))
                {
                    i++;
                }
                if (_IsSmaller(tree, n, this.heap[i], this.depth))
                {
                    break;
                }
                this.heap[k] = this.heap[i];
                k = i;
            }
            this.heap[k] = n;
        }

        private void put_bytes(byte[] p, int start, int len)
        {
            Array.Copy(p, start, this.pending, this.pendingCount, len);
            this.pendingCount += len;
        }

        internal void Reset()
        {
            this._codec.TotalBytesIn = this._codec.TotalBytesOut = 0L;
            this._codec.Message = null;
            this.pendingCount = 0;
            this.nextPending = 0;
            this.Rfc1950BytesEmitted = false;
            this.status = !this.WantRfc1950HeaderBytes ? BUSY_STATE : INIT_STATE;
            this._codec._Adler32 = Adler.Adler32(0, null, 0, 0);
            this.last_flush = 0;
            this._InitializeTreeData();
            this._InitializeLazyMatch();
        }

        internal void scan_tree(short[] tree, int max_code)
        {
            int num2 = -1;
            int num4 = tree[1];
            int num5 = 0;
            int num6 = 7;
            int num7 = 4;
            if (num4 == 0)
            {
                num6 = 0x8a;
                num7 = 3;
            }
            tree[((max_code + 1) * 2) + 1] = 0x7fff;
            for (int i = 0; i <= max_code; i++)
            {
                int num3 = num4;
                num4 = tree[((i + 1) * 2) + 1];
                if ((++num5 >= num6) || (num3 != num4))
                {
                    if (num5 < num7)
                    {
                        this.bl_tree[num3 * 2] = (short) (this.bl_tree[num3 * 2] + num5);
                    }
                    else if (num3 != 0)
                    {
                        if (num3 != num2)
                        {
                            this.bl_tree[num3 * 2] = (short) (this.bl_tree[num3 * 2] + 1);
                        }
                        this.bl_tree[InternalConstants.REP_3_6 * 2] = (short) (this.bl_tree[InternalConstants.REP_3_6 * 2] + 1);
                    }
                    else if (num5 <= 10)
                    {
                        this.bl_tree[InternalConstants.REPZ_3_10 * 2] = (short) (this.bl_tree[InternalConstants.REPZ_3_10 * 2] + 1);
                    }
                    else
                    {
                        this.bl_tree[InternalConstants.REPZ_11_138 * 2] = (short) (this.bl_tree[InternalConstants.REPZ_11_138 * 2] + 1);
                    }
                    num5 = 0;
                    num2 = num3;
                    if (num4 == 0)
                    {
                        num6 = 0x8a;
                        num7 = 3;
                    }
                    else if (num3 == num4)
                    {
                        num6 = 6;
                        num7 = 3;
                    }
                    else
                    {
                        num6 = 7;
                        num7 = 4;
                    }
                }
            }
        }

        internal void send_all_trees(int lcodes, int dcodes, int blcodes)
        {
            this.send_bits(lcodes - 0x101, 5);
            this.send_bits(dcodes - 1, 5);
            this.send_bits(blcodes - 4, 4);
            for (int i = 0; i < blcodes; i++)
            {
                this.send_bits(this.bl_tree[(ZTree.bl_order[i] * 2) + 1], 3);
            }
            this.send_tree(this.dyn_ltree, lcodes - 1);
            this.send_tree(this.dyn_dtree, dcodes - 1);
        }

        internal void send_bits(int value, int length)
        {
            int num = length;
            if (this.bi_valid > (Buf_size - num))
            {
                this.bi_buf = (short) (this.bi_buf | ((short) ((value << this.bi_valid) & 0xffff)));
                this.pending[this.pendingCount++] = (byte) this.bi_buf;
                this.pending[this.pendingCount++] = (byte) (this.bi_buf >> 8);
                this.bi_buf = (short) (value >> (Buf_size - this.bi_valid));
                this.bi_valid += num - Buf_size;
            }
            else
            {
                this.bi_buf = (short) (this.bi_buf | ((short) ((value << this.bi_valid) & 0xffff)));
                this.bi_valid += num;
            }
        }

        internal void send_code(int c, short[] tree)
        {
            int index = c * 2;
            this.send_bits(tree[index] & 0xffff, tree[index + 1] & 0xffff);
        }

        internal void send_compressed_block(short[] ltree, short[] dtree)
        {
            int num3 = 0;
            if (this.last_lit != 0)
            {
                do
                {
                    int index = this._distanceOffset + (num3 * 2);
                    int dist = ((this.pending[index] << 8) & 0xff00) | (this.pending[index + 1] & 0xff);
                    int c = this.pending[this._lengthOffset + num3] & 0xff;
                    num3++;
                    if (dist == 0)
                    {
                        this.send_code(c, ltree);
                    }
                    else
                    {
                        int num4 = ZTree.LengthCode[c];
                        this.send_code((num4 + InternalConstants.LITERALS) + 1, ltree);
                        int length = ZTree.ExtraLengthBits[num4];
                        if (length != 0)
                        {
                            c -= ZTree.LengthBase[num4];
                            this.send_bits(c, length);
                        }
                        dist--;
                        num4 = ZTree.DistanceCode(dist);
                        this.send_code(num4, dtree);
                        length = ZTree.ExtraDistanceBits[num4];
                        if (length != 0)
                        {
                            dist -= ZTree.DistanceBase[num4];
                            this.send_bits(dist, length);
                        }
                    }
                }
                while (num3 < this.last_lit);
            }
            this.send_code(END_BLOCK, ltree);
            this.last_eob_len = ltree[(END_BLOCK * 2) + 1];
        }

        internal void send_tree(short[] tree, int max_code)
        {
            int num2 = -1;
            int num4 = tree[1];
            int num5 = 0;
            int num6 = 7;
            int num7 = 4;
            if (num4 == 0)
            {
                num6 = 0x8a;
                num7 = 3;
            }
            for (int i = 0; i <= max_code; i++)
            {
                int c = num4;
                num4 = tree[((i + 1) * 2) + 1];
                if ((++num5 >= num6) || (c != num4))
                {
                    if (num5 < num7)
                    {
                        do
                        {
                            this.send_code(c, this.bl_tree);
                        }
                        while (--num5 != 0);
                    }
                    else if (c != 0)
                    {
                        if (c != num2)
                        {
                            this.send_code(c, this.bl_tree);
                            num5--;
                        }
                        this.send_code(InternalConstants.REP_3_6, this.bl_tree);
                        this.send_bits(num5 - 3, 2);
                    }
                    else if (num5 <= 10)
                    {
                        this.send_code(InternalConstants.REPZ_3_10, this.bl_tree);
                        this.send_bits(num5 - 3, 3);
                    }
                    else
                    {
                        this.send_code(InternalConstants.REPZ_11_138, this.bl_tree);
                        this.send_bits(num5 - 11, 7);
                    }
                    num5 = 0;
                    num2 = c;
                    if (num4 == 0)
                    {
                        num6 = 0x8a;
                        num7 = 3;
                    }
                    else if (c == num4)
                    {
                        num6 = 6;
                        num7 = 3;
                    }
                    else
                    {
                        num6 = 7;
                        num7 = 4;
                    }
                }
            }
        }

        internal void set_data_type()
        {
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            while (num < 7)
            {
                num3 += this.dyn_ltree[num * 2];
                num++;
            }
            while (num < 0x80)
            {
                num2 += this.dyn_ltree[num * 2];
                num++;
            }
            while (num < InternalConstants.LITERALS)
            {
                num3 += this.dyn_ltree[num * 2];
                num++;
            }
            this.data_type = (num3 <= (num2 >> 2)) ? ((sbyte) Z_ASCII) : ((sbyte) Z_BINARY);
        }

        private void SetDeflater()
        {
            switch (this.config.Flavor)
            {
                case DeflateFlavor.Store:
                    this.DeflateFunction = new CompressFunc(this.DeflateNone);
                    break;

                case DeflateFlavor.Fast:
                    this.DeflateFunction = new CompressFunc(this.DeflateFast);
                    break;

                case DeflateFlavor.Slow:
                    this.DeflateFunction = new CompressFunc(this.DeflateSlow);
                    break;
            }
        }

        internal int SetDictionary(byte[] dictionary)
        {
            int length = dictionary.Length;
            int sourceIndex = 0;
            if ((dictionary == null) || (this.status != INIT_STATE))
            {
                throw new ZlibException("Stream error.");
            }
            this._codec._Adler32 = Adler.Adler32(this._codec._Adler32, dictionary, 0, dictionary.Length);
            if (length >= MIN_MATCH)
            {
                if (length > (this.w_size - MIN_LOOKAHEAD))
                {
                    length = this.w_size - MIN_LOOKAHEAD;
                    sourceIndex = dictionary.Length - length;
                }
                Array.Copy(dictionary, sourceIndex, this.window, 0, length);
                this.strstart = length;
                this.block_start = length;
                this.ins_h = this.window[0] & 0xff;
                this.ins_h = ((this.ins_h << this.hash_shift) ^ (this.window[1] & 0xff)) & this.hash_mask;
                for (int i = 0; i <= (length - MIN_MATCH); i++)
                {
                    this.ins_h = ((this.ins_h << this.hash_shift) ^ (this.window[i + (MIN_MATCH - 1)] & 0xff)) & this.hash_mask;
                    this.prev[i & this.w_mask] = this.head[this.ins_h];
                    this.head[this.ins_h] = (short) i;
                }
            }
            return 0;
        }

        internal int SetParams(CompressionLevel level, CompressionStrategy strategy)
        {
            int num = 0;
            if (this.compressionLevel != level)
            {
                Config config = Config.Lookup(level);
                if ((config.Flavor != this.config.Flavor) && (this._codec.TotalBytesIn != 0L))
                {
                    num = this._codec.Deflate(FlushType.Partial);
                }
                this.compressionLevel = level;
                this.config = config;
                this.SetDeflater();
            }
            this.compressionStrategy = strategy;
            return num;
        }

        internal bool WantRfc1950HeaderBytes
        {
            get => 
                this._WantRfc1950HeaderBytes;
            set => 
                (this._WantRfc1950HeaderBytes = value);
        }

        internal delegate BlockState CompressFunc(FlushType flush);

        internal class Config
        {
            internal int GoodLength;
            internal int MaxLazy;
            internal int NiceLength;
            internal int MaxChainLength;
            internal DeflateFlavor Flavor;
            private static readonly DeflateManager.Config[] Table = new DeflateManager.Config[] { new DeflateManager.Config(0, 0, 0, 0, DeflateFlavor.Store), new DeflateManager.Config(4, 4, 8, 4, DeflateFlavor.Fast), new DeflateManager.Config(4, 5, 0x10, 8, DeflateFlavor.Fast), new DeflateManager.Config(4, 6, 0x20, 0x20, DeflateFlavor.Fast), new DeflateManager.Config(4, 4, 0x10, 0x10, DeflateFlavor.Slow), new DeflateManager.Config(8, 0x10, 0x20, 0x20, DeflateFlavor.Slow), new DeflateManager.Config(8, 0x10, 0x80, 0x80, DeflateFlavor.Slow), new DeflateManager.Config(8, 0x20, 0x80, 0x100, DeflateFlavor.Slow), new DeflateManager.Config(0x20, 0x80, 0x102, 0x400, DeflateFlavor.Slow), new DeflateManager.Config(0x20, 0x102, 0x102, 0x1000, DeflateFlavor.Slow) };

            private Config(int goodLength, int maxLazy, int niceLength, int maxChainLength, DeflateFlavor flavor)
            {
                this.GoodLength = goodLength;
                this.MaxLazy = maxLazy;
                this.NiceLength = niceLength;
                this.MaxChainLength = maxChainLength;
                this.Flavor = flavor;
            }

            public static DeflateManager.Config Lookup(CompressionLevel level) => 
                Table[(int) level];
        }
    }
}

