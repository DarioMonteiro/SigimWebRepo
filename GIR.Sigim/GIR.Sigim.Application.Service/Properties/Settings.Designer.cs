﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GIR.Sigim.Application.Service.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://www.sigim.srv.br/gircliente/WS/clienteSistemaBloqueioWS.asmx")]
        public string GIR_Sigim_Application_Service_clienteSistemaBloqueioWS_clienteSistemaBloqueioWS {
            get {
                return ((string)(this["GIR_Sigim_Application_Service_clienteSistemaBloqueioWS_clienteSistemaBloqueioWS"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://www.sigim.srv.br/gircliente/WS/clienteAcessoLogWS.asmx")]
        public string GIR_Sigim_Application_Service_clienteAcessoLogWS_clienteAcessoLogWS {
            get {
                return ((string)(this["GIR_Sigim_Application_Service_clienteAcessoLogWS_clienteAcessoLogWS"]));
            }
        }
    }
}