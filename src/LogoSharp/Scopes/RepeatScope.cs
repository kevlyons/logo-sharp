﻿// ==============================================================================
//  _      ____   _____  ____     _____ _    _          _____  _____
// | |    / __ \ / ____|/ __ \   / ____| |  | |   /\   |  __ \|  __ \
// | |   | |  | | |  __| |  | | | (___ | |__| |  /  \  | |__) | |__) |
// | |   | |  | | | |_ | |  | |  \___ \|  __  | / /\ \ |  _  /|  ___/
// | |___| |__| | |__| | |__| |  ____) | |  | |/ ____ \| | \ \| |
// |______\____/ \_____|\____/  |_____/|_|  |_/_/    \_\_|  \_\_|
//
// Logo Sharp, Logo programming language for managed world.
// Licensed under MIT license.
// By daxnet, https://github.com/daxnet/logo-sharp
// https://sunnycoding.cn
// ==============================================================================

namespace LogoSharp.Scopes
{
    /// <summary>
    /// Represents the scope for a REPEAT execution.
    /// </summary>
    internal sealed class RepeatScope : Scope
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <c>RepeatScope</c> class.
        /// </summary>
        /// <param name="name"></param>
        public RepeatScope(string name) : base(name) { }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets or sets the current count of the REPEAT execution, starting from 1.
        /// </summary>
        public int RepCount { get; set; }

        #endregion Public Properties
    }
}