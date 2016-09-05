﻿using NaturalDate.Text;

namespace NaturalDate
{
    public abstract class Parser
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="enumerator">The token enumerator to handle the incoming tokens.</param>
        protected Parser(TokenEnumerator enumerator)
        {
            Enumerator = enumerator;
        }

        /// <summary>
        /// Attempt to make one of the specified tokens.
        /// </summary>
        /// <param name="tokens">The list of tokens to test for a match.</param>
        /// <param name="found">The token that was found.</param>
        /// <returns>true if one of the the specific tokens was made, false if not.</returns>
        protected bool TryMakeToken(Token[] tokens, out Token found)
        {
            found = Token.None;
            
            foreach (var token in tokens)
            {
                if (TryMakeToken(token))
                {
                    found = token;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Attempt to make one of the specified tokens.
        /// </summary>
        /// <param name="tokens">The list of tokens to test for a match.</param>
        /// <returns>true if one of the the specific tokens was made, false if not.</returns>
        protected bool TryMakeToken(params Token[] tokens)
        {
            foreach (var token in tokens)
            {
                if (TryMakeToken(token))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Attempt to make the specific token.
        /// </summary>
        /// <param name="token">The specific token to match at the current point.</param>
        /// <returns>true if the specific separator token was made, false if not.</returns>
        protected bool TryMakeToken(Token token)
        {
            if (Enumerator.Peek() == token)
            {
                Enumerator.Take();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Attempt to make a numeric value whilst considering leading zeros.
        /// </summary>
        /// <param name="length">The minimum length (including leading zeros).</param>
        /// <param name="value">The value that was made.</param>
        /// <returns>true if a value with the given length could be made, false if not.</returns>
        protected bool TryMakeNumeric(int length, out int value)
        {
            return TryMakeNumeric(length, length, out value);
        }

        /// <summary>
        /// Attempt to make a numeric value whilst considering leading zeros.
        /// </summary>
        /// <param name="minLength">The minimum length (including leading zeros).</param>
        /// <param name="maxLength">The maximum length (including leading zeros).</param>
        /// <param name="value">The value that was made.</param>
        /// <returns>true if a value with the given length could be made, false if not.</returns>
        protected bool TryMakeNumeric(int minLength, int maxLength, out int value)
        {
            value = 0;

            var token = Enumerator.Peek();
            if (token.Kind == TokenKind.Number && token.Text.Length >= minLength && token.Text.Length <= maxLength)
            {
                Enumerator.Take();
                value = token.AsInteger();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Delegate for the TryMake function to allow for "out" parameters.
        /// </summary>
        /// <typeparam name="T">The type of the out parameter.</typeparam>
        /// <param name="found">The out parameter that was found during the make operation.</param>
        /// <returns>true if the make operation found a parameter, false if not.</returns>
        protected delegate bool TryMakeDelegate<T>(out T found);

        /// <summary>
        /// Try to match a function on the enumerator ensuring that the enumerator is restored on failure.
        /// </summary>
        /// <param name="make">The callback function to match on the enumerator.</param>
        /// <param name="found">The out value that was found.</param>
        /// <returns>true if the matching function successfully made a match, false if not.</returns>
        protected bool TryMake<T>(TryMakeDelegate<T> make, out T found)
        {
            using (var checkpoint = Enumerator.Checkpoint())
            {
                if (make(out found))
                {
                    return true;
                }

                checkpoint.Rollback();
            }

            return false;
        }

        /// <summary>
        /// Returns the enumerator to handle the incoming tokens.
        /// </summary>
        protected TokenEnumerator Enumerator { get; }
    }
}