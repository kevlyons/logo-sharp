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

using LogoSharp.Scopes;
using System;
using System.Collections.Generic;

namespace LogoSharp.Functions
{
    /// <summary>
    /// Represents the function that returns the square root of a given value.
    /// </summary>
    internal sealed class SqrtFunction : Function
    {
        #region Public Properties

        /// <summary>
        /// Gets a list of arguments.
        /// </summary>
        public override IEnumerable<string> Arguments => new List<string> { "p" };

        /// <summary>
        /// Gets the name of the function.
        /// </summary>
        public override string Name => "SQRT";

        #endregion Public Properties

        #region Protected Methods

        /// <summary>
        /// Executes the function invocation.
        /// </summary>
        /// <param name="procedureScope">The <see cref="ProcedureScope"/> which provides the variables
        /// in the calling procedure.</param>
        /// <returns>The execution return value.</returns>
        protected override float Execute(ProcedureScope procedureScope) => Convert.ToSingle(Math.Sqrt(Convert.ToDouble(procedureScope["p"])));

        #endregion Protected Methods
    }
}