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
using System.Collections.Generic;
using System.Text;
using Thrift.Protocol;

namespace Netfox.SnooperMessenger.Protocol
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class MNMessagesSyncImageMetadata : TBase
  {
    private int _Width;
    private int _Height;
    private Dictionary<int, string> _ImageURIMap;
    private int _ImageSource;
    private string _RawImageURI;
    private string _RawImageURIFormat;
    private Dictionary<int, string> _AnimatedImageURIMap;
    private string _ImageURIMapFormat;
    private string _AnimatedImageURIMapFormat;
    private bool _RenderAsSticker;

    public int Width
    {
      get
      {
        return _Width;
      }
      set
      {
        __isset.Width = true;
        this._Width = value;
      }
    }

    public int Height
    {
      get
      {
        return _Height;
      }
      set
      {
        __isset.Height = true;
        this._Height = value;
      }
    }

    public Dictionary<int, string> ImageURIMap
    {
      get
      {
        return _ImageURIMap;
      }
      set
      {
        __isset.ImageURIMap = true;
        this._ImageURIMap = value;
      }
    }

    public int ImageSource
    {
      get
      {
        return _ImageSource;
      }
      set
      {
        __isset.ImageSource = true;
        this._ImageSource = value;
      }
    }

    public string RawImageURI
    {
      get
      {
        return _RawImageURI;
      }
      set
      {
        __isset.RawImageURI = true;
        this._RawImageURI = value;
      }
    }

    public string RawImageURIFormat
    {
      get
      {
        return _RawImageURIFormat;
      }
      set
      {
        __isset.RawImageURIFormat = true;
        this._RawImageURIFormat = value;
      }
    }

    public Dictionary<int, string> AnimatedImageURIMap
    {
      get
      {
        return _AnimatedImageURIMap;
      }
      set
      {
        __isset.AnimatedImageURIMap = true;
        this._AnimatedImageURIMap = value;
      }
    }

    public string ImageURIMapFormat
    {
      get
      {
        return _ImageURIMapFormat;
      }
      set
      {
        __isset.ImageURIMapFormat = true;
        this._ImageURIMapFormat = value;
      }
    }

    public string AnimatedImageURIMapFormat
    {
      get
      {
        return _AnimatedImageURIMapFormat;
      }
      set
      {
        __isset.AnimatedImageURIMapFormat = true;
        this._AnimatedImageURIMapFormat = value;
      }
    }

    public bool RenderAsSticker
    {
      get
      {
        return _RenderAsSticker;
      }
      set
      {
        __isset.RenderAsSticker = true;
        this._RenderAsSticker = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool Width;
      public bool Height;
      public bool ImageURIMap;
      public bool ImageSource;
      public bool RawImageURI;
      public bool RawImageURIFormat;
      public bool AnimatedImageURIMap;
      public bool ImageURIMapFormat;
      public bool AnimatedImageURIMapFormat;
      public bool RenderAsSticker;
    }

    public MNMessagesSyncImageMetadata() {
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
              if (field.Type == TType.I32) {
                Width = iprot.ReadI32();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 2:
              if (field.Type == TType.I32) {
                Height = iprot.ReadI32();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 3:
              if (field.Type == TType.Map) {
                {
                  ImageURIMap = new Dictionary<int, string>();
                  TMap _map72 = iprot.ReadMapBegin();
                  for( int _i73 = 0; _i73 < _map72.Count; ++_i73)
                  {
                    int _key74;
                    string _val75;
                    _key74 = iprot.ReadI32();
                    _val75 = iprot.ReadString();
                    ImageURIMap[_key74] = _val75;
                  }
                  iprot.ReadMapEnd();
                }
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 4:
              if (field.Type == TType.I32) {
                ImageSource = iprot.ReadI32();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 5:
              if (field.Type == TType.String) {
                RawImageURI = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 6:
              if (field.Type == TType.String) {
                RawImageURIFormat = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 7:
              if (field.Type == TType.Map) {
                {
                  AnimatedImageURIMap = new Dictionary<int, string>();
                  TMap _map76 = iprot.ReadMapBegin();
                  for( int _i77 = 0; _i77 < _map76.Count; ++_i77)
                  {
                    int _key78;
                    string _val79;
                    _key78 = iprot.ReadI32();
                    _val79 = iprot.ReadString();
                    AnimatedImageURIMap[_key78] = _val79;
                  }
                  iprot.ReadMapEnd();
                }
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 8:
              if (field.Type == TType.String) {
                ImageURIMapFormat = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 9:
              if (field.Type == TType.String) {
                AnimatedImageURIMapFormat = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 10:
              if (field.Type == TType.Bool) {
                RenderAsSticker = iprot.ReadBool();
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
        TStruct struc = new TStruct("MNMessagesSyncImageMetadata");
        oprot.WriteStructBegin(struc);
        TField field = new TField();
        if (__isset.Width) {
          field.Name = "Width";
          field.Type = TType.I32;
          field.ID = 1;
          oprot.WriteFieldBegin(field);
          oprot.WriteI32(Width);
          oprot.WriteFieldEnd();
        }
        if (__isset.Height) {
          field.Name = "Height";
          field.Type = TType.I32;
          field.ID = 2;
          oprot.WriteFieldBegin(field);
          oprot.WriteI32(Height);
          oprot.WriteFieldEnd();
        }
        if (ImageURIMap != null && __isset.ImageURIMap) {
          field.Name = "ImageURIMap";
          field.Type = TType.Map;
          field.ID = 3;
          oprot.WriteFieldBegin(field);
          {
            oprot.WriteMapBegin(new TMap(TType.I32, TType.String, ImageURIMap.Count));
            foreach (int _iter80 in ImageURIMap.Keys)
            {
              oprot.WriteI32(_iter80);
              oprot.WriteString(ImageURIMap[_iter80]);
            }
            oprot.WriteMapEnd();
          }
          oprot.WriteFieldEnd();
        }
        if (__isset.ImageSource) {
          field.Name = "ImageSource";
          field.Type = TType.I32;
          field.ID = 4;
          oprot.WriteFieldBegin(field);
          oprot.WriteI32(ImageSource);
          oprot.WriteFieldEnd();
        }
        if (RawImageURI != null && __isset.RawImageURI) {
          field.Name = "RawImageURI";
          field.Type = TType.String;
          field.ID = 5;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(RawImageURI);
          oprot.WriteFieldEnd();
        }
        if (RawImageURIFormat != null && __isset.RawImageURIFormat) {
          field.Name = "RawImageURIFormat";
          field.Type = TType.String;
          field.ID = 6;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(RawImageURIFormat);
          oprot.WriteFieldEnd();
        }
        if (AnimatedImageURIMap != null && __isset.AnimatedImageURIMap) {
          field.Name = "AnimatedImageURIMap";
          field.Type = TType.Map;
          field.ID = 7;
          oprot.WriteFieldBegin(field);
          {
            oprot.WriteMapBegin(new TMap(TType.I32, TType.String, AnimatedImageURIMap.Count));
            foreach (int _iter81 in AnimatedImageURIMap.Keys)
            {
              oprot.WriteI32(_iter81);
              oprot.WriteString(AnimatedImageURIMap[_iter81]);
            }
            oprot.WriteMapEnd();
          }
          oprot.WriteFieldEnd();
        }
        if (ImageURIMapFormat != null && __isset.ImageURIMapFormat) {
          field.Name = "ImageURIMapFormat";
          field.Type = TType.String;
          field.ID = 8;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(ImageURIMapFormat);
          oprot.WriteFieldEnd();
        }
        if (AnimatedImageURIMapFormat != null && __isset.AnimatedImageURIMapFormat) {
          field.Name = "AnimatedImageURIMapFormat";
          field.Type = TType.String;
          field.ID = 9;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(AnimatedImageURIMapFormat);
          oprot.WriteFieldEnd();
        }
        if (__isset.RenderAsSticker) {
          field.Name = "RenderAsSticker";
          field.Type = TType.Bool;
          field.ID = 10;
          oprot.WriteFieldBegin(field);
          oprot.WriteBool(RenderAsSticker);
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
      StringBuilder __sb = new StringBuilder("MNMessagesSyncImageMetadata(");
      bool __first = true;
      if (__isset.Width) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Width: ");
        __sb.Append(Width);
      }
      if (__isset.Height) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Height: ");
        __sb.Append(Height);
      }
      if (ImageURIMap != null && __isset.ImageURIMap) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("ImageURIMap: ");
        __sb.Append(ImageURIMap);
      }
      if (__isset.ImageSource) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("ImageSource: ");
        __sb.Append(ImageSource);
      }
      if (RawImageURI != null && __isset.RawImageURI) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("RawImageURI: ");
        __sb.Append(RawImageURI);
      }
      if (RawImageURIFormat != null && __isset.RawImageURIFormat) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("RawImageURIFormat: ");
        __sb.Append(RawImageURIFormat);
      }
      if (AnimatedImageURIMap != null && __isset.AnimatedImageURIMap) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("AnimatedImageURIMap: ");
        __sb.Append(AnimatedImageURIMap);
      }
      if (ImageURIMapFormat != null && __isset.ImageURIMapFormat) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("ImageURIMapFormat: ");
        __sb.Append(ImageURIMapFormat);
      }
      if (AnimatedImageURIMapFormat != null && __isset.AnimatedImageURIMapFormat) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("AnimatedImageURIMapFormat: ");
        __sb.Append(AnimatedImageURIMapFormat);
      }
      if (__isset.RenderAsSticker) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("RenderAsSticker: ");
        __sb.Append(RenderAsSticker);
      }
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}
