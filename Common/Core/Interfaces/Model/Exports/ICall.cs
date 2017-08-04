﻿// Copyright (c) 2017 Jan Pluskal
//
//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.

using System;
using System.Collections.Generic;
using Netfox.Core.Database.Wrappers;

namespace Netfox.Core.Interfaces.Model.Exports
{
    public interface ICall : IExportBase
    {
        // who is calling
        string From { get; }
        // who is being called
        string To { get; }
        // when did the call start
        DateTime? Start { get; }
        // when did the call end
        DateTime? End { get; }
        // how long did the call take
        TimeSpan? Duration { get; }
        IEnumerable<string> PossibleCodecs { get; }
        IEnumerable<IPEndPointEF> CallStreamAddresses { get; }
        //IPEndPoint SourceEndPoint { get; }
        //IPEndPoint DestinationEndPoint { get; }

        IList<ICallStream> CallStreams { get; } 
        IList<ICallStream> PossibleCallStreams { get; } 
    }
}