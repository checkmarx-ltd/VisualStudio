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

namespace CxViewerAction2022.CxWsResolver {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="CxWSResolverSoap", Namespace="http://Checkmarx.com")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CxWSBasicRepsonse))]
    public partial class CxWSResolver : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback GetWebServiceUrlOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public CxWSResolver() {
            this.Url = global::CxViewerAction2022.Properties.Settings.Default.CxViewerAction2022_CxWsResolver_CxWSResolver;
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
        public event GetWebServiceUrlCompletedEventHandler GetWebServiceUrlCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://Checkmarx.com/GetWebServiceUrl", RequestNamespace="http://Checkmarx.com", ResponseNamespace="http://Checkmarx.com", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public CxWSResponseDiscovery GetWebServiceUrl(CxClientType ClientType, int APIVersion) {
            object[] results = this.Invoke("GetWebServiceUrl", new object[] {
                        ClientType,
                        APIVersion});
            return ((CxWSResponseDiscovery)(results[0]));
        }
        
        /// <remarks/>
        public void GetWebServiceUrlAsync(CxClientType ClientType, int APIVersion) {
            this.GetWebServiceUrlAsync(ClientType, APIVersion, null);
        }
        
        /// <remarks/>
        public void GetWebServiceUrlAsync(CxClientType ClientType, int APIVersion, object userState) {
            if ((this.GetWebServiceUrlOperationCompleted == null)) {
                this.GetWebServiceUrlOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetWebServiceUrlOperationCompleted);
            }
            this.InvokeAsync("GetWebServiceUrl", new object[] {
                        ClientType,
                        APIVersion}, this.GetWebServiceUrlOperationCompleted, userState);
        }
        
        private void OnGetWebServiceUrlOperationCompleted(object arg) {
            if ((this.GetWebServiceUrlCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetWebServiceUrlCompleted(this, new GetWebServiceUrlCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.4084.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://Checkmarx.com")]
    public enum CxClientType {
        
        /// <remarks/>
        None,
        
        /// <remarks/>
        WebPortal,
        
        /// <remarks/>
        CLI,
        
        /// <remarks/>
        Eclipse,
        
        /// <remarks/>
        VS,
        
        /// <remarks/>
        InteliJ,
        
        /// <remarks/>
        Audit,
        
        /// <remarks/>
        SDK,
        
        /// <remarks/>
        Jenkins,
        
        /// <remarks/>
        TFSBuild,
        
        /// <remarks/>
        Importer,
        
        /// <remarks/>
        Other,
        
        /// <remarks/>
        Sonar,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.4084.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://Checkmarx.com")]
    public partial class CxWSResponseDiscovery : CxWSBasicRepsonse {
        
        private string serviceURLField;
        
        /// <remarks/>
        public string ServiceURL {
            get {
                return this.serviceURLField;
            }
            set {
                this.serviceURLField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CxWSResponseDiscovery))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.4084.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://Checkmarx.com")]
    public partial class CxWSBasicRepsonse {
        
        private bool isSuccesfullField;
        
        private string errorMessageField;
        
        /// <remarks/>
        public bool IsSuccesfull {
            get {
                return this.isSuccesfullField;
            }
            set {
                this.isSuccesfullField = value;
            }
        }
        
        /// <remarks/>
        public string ErrorMessage {
            get {
                return this.errorMessageField;
            }
            set {
                this.errorMessageField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    public delegate void GetWebServiceUrlCompletedEventHandler(object sender, GetWebServiceUrlCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetWebServiceUrlCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetWebServiceUrlCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public CxWSResponseDiscovery Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((CxWSResponseDiscovery)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591