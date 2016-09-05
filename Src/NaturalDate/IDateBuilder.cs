﻿namespace NaturalDate
{
    public interface IDateBuilder<out T> : IDateBuilder
    {
        /// <summary>
        /// Returns the date that has been built.
        /// </summary>
        /// <returns>The date that has been built.</returns>
        T Build();
    }

    public interface IDateBuilder
    {
        /// <summary>
        /// Gets or sets the day portion of the date.
        /// </summary>
        int Day { get; set; }

        /// <summary>
        /// Gets or sets the month portion of the date.
        /// </summary>
        int Month { get; set; }

        /// <summary>
        /// Gets or sets the year portion of the date.
        /// </summary>
        int Year { get; set; }
    }
}