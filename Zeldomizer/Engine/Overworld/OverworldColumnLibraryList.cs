﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Overworld
{
    /// <summary>
    /// Represents a list of lists of overworld columns, in raw form.
    /// </summary>
    /// <remarks>
    /// Refer to <see cref="Underworld.UnderworldColumnLibraryList"/> for more information
    /// about this kind of list.
    /// </remarks>
    public class OverworldColumnLibraryList : IReadOnlyList<OverworldColumnLibrary>
    {
        private readonly ISource _source;
        private readonly IReadOnlyList<int> _offsets;

        /// <summary>
        /// Initialize a list of lists of overworld columns.
        /// </summary>
        public OverworldColumnLibraryList(ISource source, IReadOnlyList<int> offsets)
        {
            _source = source;
            _offsets = offsets;
        }

        /// <summary>
        /// Enumerate all overworld column lists.
        /// </summary>
        public IEnumerator<OverworldColumnLibrary> GetEnumerator()
        {
            return Enumerable
                .Range(0, Count)
                .Select(i => this[i])
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Get the number of overworld column lists.
        /// </summary>
        public int Count => 16;

        /// <summary>
        /// Get the overworld column list at the specified index.
        /// </summary>
        public OverworldColumnLibrary this[int index] => 
            new OverworldColumnLibrary(new SourceBlock(_source, _offsets[index]));
    }
}
