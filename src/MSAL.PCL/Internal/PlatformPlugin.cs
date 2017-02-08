//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using Microsoft.Identity.Client.Internal.Interfaces;

namespace Microsoft.Identity.Client.Internal
{
    internal static class PlatformPluginSwitch
    {
        static PlatformPluginSwitch()
        {
            DynamicallyLinkAssembly = true;
        }

        public static bool DynamicallyLinkAssembly { get; set; }
    }

    internal static class PlatformPlugin
    {
        private const string Namespace = "Microsoft.Identity.Client.";

        static PlatformPlugin()
        {
            if (PlatformPluginSwitch.DynamicallyLinkAssembly)
            {
                InitializeByAssemblyDynamicLinking();
            }
        }

        public static IWebUIFactory WebUIFactory { get; set; }

        public static ITokenCachePlugin NewTokenCachePluginInstance
        {
            get
            {
#if !NETSTANDARD1_1
                return (ITokenCachePlugin) new TokenCachePlugin();
#else
                return null;
#endif
            }
        }

        public static ITokenCachePlugin TokenCachePlugin { get; set; }
        public static LoggerBase Logger { get; set; }
        public static PlatformInformationBase PlatformInformation { get; set; }
        public static ICryptographyHelper CryptographyHelper { get; set; }
        public static IDeviceAuthHelper DeviceAuthHelper { get; set; }
        public static IBrokerHelper BrokerHelper { get; set; }
        public static IPlatformParameters DefaultPlatformParameters { get; set; }

        public static void InitializeByAssemblyDynamicLinking()
        {
#if !NETSTANDARD1_1
            InjectDependecies(
                (IWebUIFactory) new WebUIFactory(),
                (ITokenCachePlugin) new TokenCachePlugin(),
                (LoggerBase) new Logger(),
                (PlatformInformationBase) new PlatformInformation(),
                (ICryptographyHelper) new CryptographyHelper(),
                (IDeviceAuthHelper) new DeviceAuthHelper(),
                (IBrokerHelper) new BrokerHelper(),
                (IPlatformParameters) new PlatformParameters());
#endif
        }

        public static void InjectDependecies(IWebUIFactory webUIFactory, ITokenCachePlugin tokenCachePlugin,
            LoggerBase logger,
            PlatformInformationBase platformInformation, ICryptographyHelper cryptographyHelper,
            IDeviceAuthHelper deviceAuthHelper, IBrokerHelper brokerHelper, IPlatformParameters platformParameters)
        {
            WebUIFactory = webUIFactory;
            TokenCachePlugin = tokenCachePlugin;
            Logger = logger;
            PlatformInformation = platformInformation;
            CryptographyHelper = cryptographyHelper;
            DeviceAuthHelper = deviceAuthHelper;
            BrokerHelper = brokerHelper;
            DefaultPlatformParameters = platformParameters;
        }
    }
}