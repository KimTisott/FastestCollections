﻿using System;
using System.Collections.Generic;

namespace FastCollections
{
    public class FastHashSet
    {
        private int[] dictionaries;
        private int BufferSize => dictionaries.Length * 32;

        public FastHashSet()
        {
            Clear();
        }

        public void Add(int index)
        {
            if (index < 0)
                return;

            var dictionaryIndex = index >> 5;

            CheckDictionarySize(dictionaryIndex);

            dictionaries[dictionaryIndex] |= 1 << (index % 32);
        }

        public void Clear()
        {
            dictionaries = new int[1];
        }

        public bool Contains(int index)
        {
            if (index < 0 || index >= BufferSize)
                return false;

            return ((1 << (index % 32)) & dictionaries[index >> 5]) != 0;
        }

        public void Remove(int index)
        {
            if (index < 0 || index > BufferSize)
                return;

            dictionaries[index >> 5] ^= 1 << (index % 32);
        }

        private void CheckDictionarySize(int dictionaryIndex)
        {
            var necessarySize = dictionaryIndex + 1;

            if (necessarySize < dictionaries.Length)
                return;

            var doubleSize = dictionaries.Length * 2;

            Array.Resize(ref dictionaries, necessarySize > doubleSize ? necessarySize : doubleSize);
        }
    }

    public static class FastSetExtensions
    {
        public static FastHashSet ToFastSet(this IEnumerable<int> enumerable)
        {
            var fastSet = new FastHashSet();

            foreach (var item in enumerable)
            {
                fastSet.Add(item);
            }

            return fastSet;
        }
    }
}