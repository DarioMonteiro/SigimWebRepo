﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace GIR.Sigim.Application.Service.clienteSistemaBloqueioWS {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1038.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="clienteSistemaBloqueioWSSoap", Namespace="http://tempuri.org/")]
    public partial class clienteSistemaBloqueioWS : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback RecuperaPorClienteOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public clienteSistemaBloqueioWS() {
            this.Url = global::GIR.Sigim.Application.Service.Properties.Settings.Default.GIR_Sigim_Application_Service_clienteSistemaBloqueioWS_clienteSistemaBloqueioWS;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event RecuperaPorClienteCompletedEventHandler RecuperaPorClienteCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/RecuperaPorCliente", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public clienteSistemaBloqueioSIGIMWS[] RecuperaPorCliente(int parIntCliente) {
            object[] results = this.Invoke("RecuperaPorCliente", new object[] {
                        parIntCliente});
            return ((clienteSistemaBloqueioSIGIMWS[])(results[0]));
        }
        
        /// <remarks/>
        public void RecuperaPorClienteAsync(int parIntCliente) {
            this.RecuperaPorClienteAsync(parIntCliente, null);
        }
        
        /// <remarks/>
        public void RecuperaPorClienteAsync(int parIntCliente, object userState) {
            if ((this.RecuperaPorClienteOperationCompleted == null)) {
                this.RecuperaPorClienteOperationCompleted = new System.Threading.SendOrPostCallback(this.OnRecuperaPorClienteOperationCompleted);
            }
            this.InvokeAsync("RecuperaPorCliente", new object[] {
                        parIntCliente}, this.RecuperaPorClienteOperationCompleted, userState);
        }
        
        private void OnRecuperaPorClienteOperationCompleted(object arg) {
            if ((this.RecuperaPorClienteCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.RecuperaPorClienteCompleted(this, new RecuperaPorClienteCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class clienteSistemaBloqueioSIGIMWS {
        
        private int codigoSistemaField;
        
        private string descricaoSistemaField;
        
        private string nomeInternoSistemaField;
        
        /// <remarks/>
        public int codigoSistema {
            get {
                return this.codigoSistemaField;
            }
            set {
                this.codigoSistemaField = value;
            }
        }
        
        /// <remarks/>
        public string descricaoSistema {
            get {
                return this.descricaoSistemaField;
            }
            set {
                this.descricaoSistemaField = value;
            }
        }
        
        /// <remarks/>
        public string nomeInternoSistema {
            get {
                return this.nomeInternoSistemaField;
            }
            set {
                this.nomeInternoSistemaField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1038.0")]
    public delegate void RecuperaPorClienteCompletedEventHandler(object sender, RecuperaPorClienteCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1038.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class RecuperaPorClienteCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal RecuperaPorClienteCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public clienteSistemaBloqueioSIGIMWS[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((clienteSistemaBloqueioSIGIMWS[])(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591