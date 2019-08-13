﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AspectRegistrationBuilderTests.cs" company="CBRE">
//   
// </copyright>
//  <summary>
//   The aspect registration builder tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using AspectCentral.Abstractions.Configuration;
using AspectCentral.Abstractions.Logging;
using AspectCentral.Abstractions.Profiling;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace AspectCentral.Abstractions.Tests
{
    /// <summary>
    /// The aspect registration builder tests.
    /// </summary>
    public class AspectRegistrationBuilderTests
    {
        /// <summary>
        /// The add aspect success.
        /// </summary>
        [Fact]
        public void AddAspectSuccess()
        {
            var aspectRegistrationBuilder = new ServiceCollection().AddAspectSupport().AddService(typeof(ITestInterface), typeof(MyTestInterface), ServiceLifetime.Scoped)
                .AddAspect(LoggingAspectFactory.LoggingAspectFactoryType, null, typeof(MyTestInterface).GetMethods()).AddAspect(
                    ProfilingAspectFactory.ProfilingAspectFactoryType,
                    null,
                    typeof(MyTestInterface).GetMethods());
            var aspects = aspectRegistrationBuilder.AspectConfigurationProvider.ConfigurationEntries[0].GetAspects();

            aspects.Count().Should().Be(2);
        }

        /// <summary>
        /// The add aspect throws argument exception when aspect factory does not implement i aspect factory.
        /// </summary>
        [Fact]
        public void AddAspectThrowsArgumentExceptionWhenAspectFactoryDoesNotImplementIAspectFactory()
        {
            var aspectRegistrationBuilder = new AspectRegistrationBuilder(new ServiceCollection(), new InMemoryAspectConfigurationProvider());
            Assert.Throws<ArgumentException>(() => aspectRegistrationBuilder.AddAspect(GetType(), default));
        }

        /// <summary>
        /// The add aspect throws argument null exception when aspect factory is null.
        /// </summary>
        [Fact]
        public void AddAspectThrowsArgumentNullExceptionWhenAspectFactoryIsNull()
        {
            var aspectRegistrationBuilder = new AspectRegistrationBuilder(new ServiceCollection(), new InMemoryAspectConfigurationProvider());
            Assert.Throws<ArgumentNullException>(() => aspectRegistrationBuilder.AddAspect(default, default));
        }

        /// <summary>
        /// The add aspect throws invalid operation exception when services have been registered.
        /// </summary>
        [Fact]
        public void AddAspectThrowsInvalidOperationExceptionWhenServicesHaveBeenRegistered()
        {
            var aspectRegistrationBuilder = new AspectRegistrationBuilder(new ServiceCollection(), new InMemoryAspectConfigurationProvider());
            Assert.Throws<InvalidOperationException>(() => aspectRegistrationBuilder.AddAspect(LoggingAspectFactory.LoggingAspectFactoryType, default));
        }

        /// <summary>
        /// The add aspect with factory success.
        /// </summary>
        [Fact]
        public void AddAspectWithFactorySuccess()
        {
            var aspectRegistrationBuilder = new ServiceCollection().AddAspectSupport().AddService(typeof(ITestInterface), serviceProvider => new MyTestInterface(), ServiceLifetime.Scoped)
                .AddAspect(LoggingAspectFactory.LoggingAspectFactoryType, null, typeof(MyTestInterface).GetMethods());
            var aspects = aspectRegistrationBuilder.AspectConfigurationProvider.ConfigurationEntries[0].GetAspects();
            aspects.Count().Should().Be(1);
        }

        /// <summary>
        /// The add service success.
        /// </summary>
        [Fact]
        public void AddServiceSuccess()
        {
            var aspectRegistrationBuilder = new AspectRegistrationBuilder(new ServiceCollection(), new InMemoryAspectConfigurationProvider());
            aspectRegistrationBuilder.AddService(typeof(IAspectFactory), LoggingAspectFactory.LoggingAspectFactoryType, ServiceLifetime.Scoped);
            aspectRegistrationBuilder.Services.Count.Should().Be(2);
            aspectRegistrationBuilder.AspectConfigurationProvider.ConfigurationEntries.Count.Should().Be(1);
            aspectRegistrationBuilder.AspectConfigurationProvider.ConfigurationEntries[0].ServiceDescriptor.ImplementationType.Should().Be(LoggingAspectFactory.LoggingAspectFactoryType);
        }

        /// <summary>
        /// The add service throws argument null exception when implementation does not implement service.
        /// </summary>
        [Fact]
        public void AddServiceThrowsArgumentNullExceptionWhenImplementationDoesNotImplementService()
        {
            var aspectRegistrationBuilder = new AspectRegistrationBuilder(new ServiceCollection(), new InMemoryAspectConfigurationProvider());
            Assert.Throws<ArgumentException>(() => aspectRegistrationBuilder.AddService(typeof(IAspectConfigurationProvider), GetType(), ServiceLifetime.Scoped));
        }

        /// <summary>
        /// The add service throws argument null exception when implementation is null.
        /// </summary>
        [Fact]
        public void AddServiceThrowsArgumentNullExceptionWhenImplementationIsNull()
        {
            var aspectRegistrationBuilder = new AspectRegistrationBuilder(new ServiceCollection(), new InMemoryAspectConfigurationProvider());
            Assert.Throws<ArgumentNullException>(() => aspectRegistrationBuilder.AddService(typeof(IAspectConfigurationProvider), default(Type), ServiceLifetime.Scoped));
        }

        /// <summary>
        /// The add service throws argument null exception when service is null.
        /// </summary>
        [Fact]
        public void AddServiceThrowsArgumentNullExceptionWhenServiceIsNull()
        {
            var aspectRegistrationBuilder = new AspectRegistrationBuilder(new ServiceCollection(), new InMemoryAspectConfigurationProvider());
            Assert.Throws<ArgumentNullException>(() => aspectRegistrationBuilder.AddService(null, default(Type), ServiceLifetime.Scoped));
        }

        /// <summary>
        /// The add service with factory success.
        /// </summary>
        [Fact]
        public void AddServiceWithFactorySuccess()
        {
            var aspectRegistrationBuilder = new AspectRegistrationBuilder(new ServiceCollection(), new InMemoryAspectConfigurationProvider());
            aspectRegistrationBuilder.AddService(
                typeof(IAspectFactory),
                provider => new LoggingAspectFactory(provider.GetService<ILoggerFactory>(), provider.GetService<IAspectConfigurationProvider>()),
                ServiceLifetime.Scoped);
            aspectRegistrationBuilder.Services.Count.Should().Be(1);
            aspectRegistrationBuilder.AspectConfigurationProvider.ConfigurationEntries.Count.Should().Be(1);
            aspectRegistrationBuilder.AspectConfigurationProvider.ConfigurationEntries[0].ServiceDescriptor.ImplementationFactory.Should().NotBeNull();
            aspectRegistrationBuilder.AspectConfigurationProvider.ConfigurationEntries[0].ServiceDescriptor.ImplementationType.Should().BeNull();
        }

        /// <summary>
        /// The add service with factory throws argument null exception when implementation is null.
        /// </summary>
        [Fact]
        public void AddServiceWithFactoryThrowsArgumentNullExceptionWhenImplementationIsNull()
        {
            var aspectRegistrationBuilder = new AspectRegistrationBuilder(new ServiceCollection(), new InMemoryAspectConfigurationProvider());
            Assert.Throws<ArgumentNullException>(() => aspectRegistrationBuilder.AddService(typeof(IAspectFactory), default(Func<IServiceProvider, object>), ServiceLifetime.Scoped));
        }

        /// <summary>
        /// The add service with factory throws argument null exception when service is null.
        /// </summary>
        [Fact]
        public void AddServiceWithFactoryThrowsArgumentNullExceptionWhenServiceIsNull()
        {
            var aspectRegistrationBuilder = new AspectRegistrationBuilder(new ServiceCollection(), new InMemoryAspectConfigurationProvider());
            Assert.Throws<ArgumentNullException>(() => aspectRegistrationBuilder.AddService(null, default(Func<IServiceProvider, object>), ServiceLifetime.Scoped));
        }

        /// <summary>
        /// The constructor creates new object.
        /// </summary>
        [Fact]
        public void ConstructorCreatesNewObject()
        {
            var aspectRegistrationBuilder = new AspectRegistrationBuilder(new ServiceCollection(), new InMemoryAspectConfigurationProvider());
            aspectRegistrationBuilder.Should().NotBeNull();
        }

        /// <summary>
        /// The constructor throws argument null exception when aspect configuration provider is null.
        /// </summary>
        [Fact]
        public void ConstructorThrowsArgumentNullExceptionWhenAspectConfigurationProviderIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new AspectRegistrationBuilder(new ServiceCollection(), null));
        }

        /// <summary>
        /// The constructor throws argument null exception when services is null.
        /// </summary>
        [Fact]
        public void ConstructorThrowsArgumentNullExceptionWhenServicesIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new AspectRegistrationBuilder(null, null));
        }
    }
}