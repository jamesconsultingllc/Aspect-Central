﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProfilingAspectRegistrationBuilderExtensions.cs" company="CBRE">
//   
// </copyright>
// <summary>
//   The profiling aspect registration builder extensions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Reflection;

namespace AspectCentral.Abstractions.Profiling
{
    /// <summary>
    ///     The profiling aspect registration builder extensions.
    /// </summary>
    public static class ProfilingAspectRegistrationBuilderExtensions
    {
        /// <summary>
        /// The with logging.
        /// </summary>
        /// <param name="aspectRegistrationBuilder">
        /// The aspect registration builder.
        /// </param>
        /// <param name="methodsToIntercept">
        /// The methods To Intercept.
        /// </param>
        /// <returns>
        /// The <see cref="IAspectRegistrationBuilder"/>.
        /// </returns>
        public static IAspectRegistrationBuilder AddProfilingAspect(this IAspectRegistrationBuilder aspectRegistrationBuilder, params MethodInfo[] methodsToIntercept)
        {
            if (aspectRegistrationBuilder == null) throw new ArgumentNullException(nameof(aspectRegistrationBuilder));

            aspectRegistrationBuilder.AddAspect(ProfilingAspectFactory.ProfilingAspectFactoryType, methodsToIntercept: methodsToIntercept);
            return aspectRegistrationBuilder;
        }
    }
}