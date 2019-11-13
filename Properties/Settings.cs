// Decompiled with JetBrains decompiler
// Type: SSTap.Properties.Settings
// Assembly: SS-TAP_对接91, Version=30.5.26.2, Culture=neutral, PublicKeyToken=null
// MVID: 3FC77BE2-506D-4E87-81A5-F87143593C29
// Assembly location: C:\Program Files (x86)\Kaguya\SS-TAP_对接91.exe

using System.CodeDom.Compiler;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace SSTap.Properties
{
  [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.3.0.0")]
  [CompilerGenerated]
  internal sealed class Settings : ApplicationSettingsBase
  {
    private static Settings defaultInstance = (Settings) SettingsBase.Synchronized((SettingsBase) new Settings());

    public static Settings Default
    {
      get
      {
        Settings defaultInstance = Settings.defaultInstance;
        return defaultInstance;
      }
    }
  }
}
