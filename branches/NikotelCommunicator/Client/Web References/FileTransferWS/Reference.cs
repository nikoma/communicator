﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.4927
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 2.0.50727.4927.
// 
#pragma warning disable 1591

namespace Remwave.Client.FileTransferWS {
    using System.Diagnostics;
    using System.Web.Services;
    using System.ComponentModel;
    using System.Web.Services.Protocols;
    using System;
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.4927")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="ServiceSoap", Namespace="http://remwave.com/FileTransfer/")]
    public partial class Service : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback GetFileSizeOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetFileOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetFileChunkOperationCompleted;
        
        private System.Threading.SendOrPostCallback PutFileSizeOperationCompleted;
        
        private System.Threading.SendOrPostCallback PutFileOperationCompleted;
        
        private System.Threading.SendOrPostCallback PutFileChunkOperationCompleted;
        
        private System.Threading.SendOrPostCallback CheckFileHashOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public Service() {
            this.Url = "http://api.remwave.com/FileTransferWS/Service.asmx";
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
        public event GetFileSizeCompletedEventHandler GetFileSizeCompleted;
        
        /// <remarks/>
        public event GetFileCompletedEventHandler GetFileCompleted;
        
        /// <remarks/>
        public event GetFileChunkCompletedEventHandler GetFileChunkCompleted;
        
        /// <remarks/>
        public event PutFileSizeCompletedEventHandler PutFileSizeCompleted;
        
        /// <remarks/>
        public event PutFileCompletedEventHandler PutFileCompleted;
        
        /// <remarks/>
        public event PutFileChunkCompletedEventHandler PutFileChunkCompleted;
        
        /// <remarks/>
        public event CheckFileHashCompletedEventHandler CheckFileHashCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://remwave.com/FileTransfer/GetFileSize", RequestNamespace="http://remwave.com/FileTransfer/", ResponseNamespace="http://remwave.com/FileTransfer/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public long GetFileSize(string username, string password, string id) {
            object[] results = this.Invoke("GetFileSize", new object[] {
                        username,
                        password,
                        id});
            return ((long)(results[0]));
        }
        
        /// <remarks/>
        public void GetFileSizeAsync(string username, string password, string id) {
            this.GetFileSizeAsync(username, password, id, null);
        }
        
        /// <remarks/>
        public void GetFileSizeAsync(string username, string password, string id, object userState) {
            if ((this.GetFileSizeOperationCompleted == null)) {
                this.GetFileSizeOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetFileSizeOperationCompleted);
            }
            this.InvokeAsync("GetFileSize", new object[] {
                        username,
                        password,
                        id}, this.GetFileSizeOperationCompleted, userState);
        }
        
        private void OnGetFileSizeOperationCompleted(object arg) {
            if ((this.GetFileSizeCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetFileSizeCompleted(this, new GetFileSizeCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://remwave.com/FileTransfer/GetFile", RequestNamespace="http://remwave.com/FileTransfer/", ResponseNamespace="http://remwave.com/FileTransfer/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")]
        public byte[] GetFile(string username, string password, string id) {
            object[] results = this.Invoke("GetFile", new object[] {
                        username,
                        password,
                        id});
            return ((byte[])(results[0]));
        }
        
        /// <remarks/>
        public void GetFileAsync(string username, string password, string id) {
            this.GetFileAsync(username, password, id, null);
        }
        
        /// <remarks/>
        public void GetFileAsync(string username, string password, string id, object userState) {
            if ((this.GetFileOperationCompleted == null)) {
                this.GetFileOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetFileOperationCompleted);
            }
            this.InvokeAsync("GetFile", new object[] {
                        username,
                        password,
                        id}, this.GetFileOperationCompleted, userState);
        }
        
        private void OnGetFileOperationCompleted(object arg) {
            if ((this.GetFileCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetFileCompleted(this, new GetFileCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://remwave.com/FileTransfer/GetFileChunk", RequestNamespace="http://remwave.com/FileTransfer/", ResponseNamespace="http://remwave.com/FileTransfer/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")]
        public byte[] GetFileChunk(string username, string password, string id, long offset, int size) {
            object[] results = this.Invoke("GetFileChunk", new object[] {
                        username,
                        password,
                        id,
                        offset,
                        size});
            return ((byte[])(results[0]));
        }
        
        /// <remarks/>
        public void GetFileChunkAsync(string username, string password, string id, long offset, int size) {
            this.GetFileChunkAsync(username, password, id, offset, size, null);
        }
        
        /// <remarks/>
        public void GetFileChunkAsync(string username, string password, string id, long offset, int size, object userState) {
            if ((this.GetFileChunkOperationCompleted == null)) {
                this.GetFileChunkOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetFileChunkOperationCompleted);
            }
            this.InvokeAsync("GetFileChunk", new object[] {
                        username,
                        password,
                        id,
                        offset,
                        size}, this.GetFileChunkOperationCompleted, userState);
        }
        
        private void OnGetFileChunkOperationCompleted(object arg) {
            if ((this.GetFileChunkCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetFileChunkCompleted(this, new GetFileChunkCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://remwave.com/FileTransfer/PutFileSize", RequestNamespace="http://remwave.com/FileTransfer/", ResponseNamespace="http://remwave.com/FileTransfer/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void PutFileSize(string username, string password, string id, long size) {
            this.Invoke("PutFileSize", new object[] {
                        username,
                        password,
                        id,
                        size});
        }
        
        /// <remarks/>
        public void PutFileSizeAsync(string username, string password, string id, long size) {
            this.PutFileSizeAsync(username, password, id, size, null);
        }
        
        /// <remarks/>
        public void PutFileSizeAsync(string username, string password, string id, long size, object userState) {
            if ((this.PutFileSizeOperationCompleted == null)) {
                this.PutFileSizeOperationCompleted = new System.Threading.SendOrPostCallback(this.OnPutFileSizeOperationCompleted);
            }
            this.InvokeAsync("PutFileSize", new object[] {
                        username,
                        password,
                        id,
                        size}, this.PutFileSizeOperationCompleted, userState);
        }
        
        private void OnPutFileSizeOperationCompleted(object arg) {
            if ((this.PutFileSizeCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.PutFileSizeCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://remwave.com/FileTransfer/PutFile", RequestNamespace="http://remwave.com/FileTransfer/", ResponseNamespace="http://remwave.com/FileTransfer/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void PutFile(string username, string password, string id, [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")] byte[] buffer) {
            this.Invoke("PutFile", new object[] {
                        username,
                        password,
                        id,
                        buffer});
        }
        
        /// <remarks/>
        public void PutFileAsync(string username, string password, string id, byte[] buffer) {
            this.PutFileAsync(username, password, id, buffer, null);
        }
        
        /// <remarks/>
        public void PutFileAsync(string username, string password, string id, byte[] buffer, object userState) {
            if ((this.PutFileOperationCompleted == null)) {
                this.PutFileOperationCompleted = new System.Threading.SendOrPostCallback(this.OnPutFileOperationCompleted);
            }
            this.InvokeAsync("PutFile", new object[] {
                        username,
                        password,
                        id,
                        buffer}, this.PutFileOperationCompleted, userState);
        }
        
        private void OnPutFileOperationCompleted(object arg) {
            if ((this.PutFileCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.PutFileCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://remwave.com/FileTransfer/PutFileChunk", RequestNamespace="http://remwave.com/FileTransfer/", ResponseNamespace="http://remwave.com/FileTransfer/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void PutFileChunk(string username, string password, string id, [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")] byte[] buffer, long offset) {
            this.Invoke("PutFileChunk", new object[] {
                        username,
                        password,
                        id,
                        buffer,
                        offset});
        }
        
        /// <remarks/>
        public void PutFileChunkAsync(string username, string password, string id, byte[] buffer, long offset) {
            this.PutFileChunkAsync(username, password, id, buffer, offset, null);
        }
        
        /// <remarks/>
        public void PutFileChunkAsync(string username, string password, string id, byte[] buffer, long offset, object userState) {
            if ((this.PutFileChunkOperationCompleted == null)) {
                this.PutFileChunkOperationCompleted = new System.Threading.SendOrPostCallback(this.OnPutFileChunkOperationCompleted);
            }
            this.InvokeAsync("PutFileChunk", new object[] {
                        username,
                        password,
                        id,
                        buffer,
                        offset}, this.PutFileChunkOperationCompleted, userState);
        }
        
        private void OnPutFileChunkOperationCompleted(object arg) {
            if ((this.PutFileChunkCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.PutFileChunkCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://remwave.com/FileTransfer/CheckFileHash", RequestNamespace="http://remwave.com/FileTransfer/", ResponseNamespace="http://remwave.com/FileTransfer/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string CheckFileHash(string username, string password, string id) {
            object[] results = this.Invoke("CheckFileHash", new object[] {
                        username,
                        password,
                        id});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void CheckFileHashAsync(string username, string password, string id) {
            this.CheckFileHashAsync(username, password, id, null);
        }
        
        /// <remarks/>
        public void CheckFileHashAsync(string username, string password, string id, object userState) {
            if ((this.CheckFileHashOperationCompleted == null)) {
                this.CheckFileHashOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCheckFileHashOperationCompleted);
            }
            this.InvokeAsync("CheckFileHash", new object[] {
                        username,
                        password,
                        id}, this.CheckFileHashOperationCompleted, userState);
        }
        
        private void OnCheckFileHashOperationCompleted(object arg) {
            if ((this.CheckFileHashCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CheckFileHashCompleted(this, new CheckFileHashCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.4927")]
    public delegate void GetFileSizeCompletedEventHandler(object sender, GetFileSizeCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.4927")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetFileSizeCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetFileSizeCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public long Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((long)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.4927")]
    public delegate void GetFileCompletedEventHandler(object sender, GetFileCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.4927")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetFileCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetFileCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public byte[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((byte[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.4927")]
    public delegate void GetFileChunkCompletedEventHandler(object sender, GetFileChunkCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.4927")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetFileChunkCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetFileChunkCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public byte[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((byte[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.4927")]
    public delegate void PutFileSizeCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.4927")]
    public delegate void PutFileCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.4927")]
    public delegate void PutFileChunkCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.4927")]
    public delegate void CheckFileHashCompletedEventHandler(object sender, CheckFileHashCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.4927")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CheckFileHashCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal CheckFileHashCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591