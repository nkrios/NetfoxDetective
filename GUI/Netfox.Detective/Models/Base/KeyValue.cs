﻿// Copyright (c) 2017 Jan Pluskal, Martin Mares, Martin Kmet
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

namespace Netfox.Detective.Models.Base
{
    /// <summary>
    ///     Modifiable key value pair.
    /// </summary>
    /// <typeparam name="TKeyType"></typeparam>
    /// <typeparam name="TValueType"></typeparam>
    public class KeyValue<TKeyType, TValueType>
    {
        #region Constructors
        public KeyValue() { }

        public KeyValue(TKeyType key, TValueType value)
        {
            this.Key = key;
            this.Value = value;
        }

        public KeyValue(TKeyType key, TValueType value, object oRef)
        {
            this.Key = key;
            this.Value = value;
            this.Ref = oRef;
        }
        #endregion

        #region Properties
        public object Ref { get; private set; }
        public TKeyType Key { get; private set; }
        public TValueType Value { get; set; }
        #endregion
    }
}