// Copyright (c) 2017 Jan Pluskal, Viliam Letavay
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

/**
 * Autogenerated by Thrift Compiler (0.9.3)
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 *  @generated
 */
using System;
using System.Text;
using Thrift.Protocol;

namespace Netfox.SnooperMessenger.Protocol
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class MNMessagesSyncDeltaReplaceMessage : TBase
  {
    private MNMessagesSyncDeltaNewMessage _NewMessage;
    private string _ReplacedMessageId;

    public MNMessagesSyncDeltaNewMessage NewMessage
    {
      get
      {
        return _NewMessage;
      }
      set
      {
        __isset.NewMessage = true;
        this._NewMessage = value;
      }
    }

    public string ReplacedMessageId
    {
      get
      {
        return _ReplacedMessageId;
      }
      set
      {
        __isset.ReplacedMessageId = true;
        this._ReplacedMessageId = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool NewMessage;
      public bool ReplacedMessageId;
    }

    public MNMessagesSyncDeltaReplaceMessage() {
    }

    public void Read (TProtocol iprot)
    {
      iprot.IncrementRecursionDepth();
      try
      {
        TField field;
        iprot.ReadStructBegin();
        while (true)
        {
          field = iprot.ReadFieldBegin();
          if (field.Type == TType.Stop) { 
            break;
          }
          switch (field.ID)
          {
            case 1:
              if (field.Type == TType.Struct) {
                NewMessage = new MNMessagesSyncDeltaNewMessage();
                NewMessage.Read(iprot);
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 2:
              if (field.Type == TType.String) {
                ReplacedMessageId = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            default: 
              TProtocolUtil.Skip(iprot, field.Type);
              break;
          }
          iprot.ReadFieldEnd();
        }
        iprot.ReadStructEnd();
      }
      finally
      {
        iprot.DecrementRecursionDepth();
      }
    }

    public void Write(TProtocol oprot) {
      oprot.IncrementRecursionDepth();
      try
      {
        TStruct struc = new TStruct("MNMessagesSyncDeltaReplaceMessage");
        oprot.WriteStructBegin(struc);
        TField field = new TField();
        if (NewMessage != null && __isset.NewMessage) {
          field.Name = "NewMessage";
          field.Type = TType.Struct;
          field.ID = 1;
          oprot.WriteFieldBegin(field);
          NewMessage.Write(oprot);
          oprot.WriteFieldEnd();
        }
        if (ReplacedMessageId != null && __isset.ReplacedMessageId) {
          field.Name = "ReplacedMessageId";
          field.Type = TType.String;
          field.ID = 2;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(ReplacedMessageId);
          oprot.WriteFieldEnd();
        }
        oprot.WriteFieldStop();
        oprot.WriteStructEnd();
      }
      finally
      {
        oprot.DecrementRecursionDepth();
      }
    }

    public override string ToString() {
      StringBuilder __sb = new StringBuilder("MNMessagesSyncDeltaReplaceMessage(");
      bool __first = true;
      if (NewMessage != null && __isset.NewMessage) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("NewMessage: ");
        __sb.Append(NewMessage== null ? "<null>" : NewMessage.ToString());
      }
      if (ReplacedMessageId != null && __isset.ReplacedMessageId) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("ReplacedMessageId: ");
        __sb.Append(ReplacedMessageId);
      }
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}
