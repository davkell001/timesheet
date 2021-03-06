﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MBTimeSheetWebApp.LoginService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="LoginService", ConfigurationName="LoginService.LoginServiceSoap")]
    public interface LoginServiceSoap {
        
        // CODEGEN: Generating message contract since the wrapper namespace (http://www.sap.com/SBO/DIS) of message LoginResponse does not match the default value (LoginService)
        [System.ServiceModel.OperationContractAttribute(Action="LoginService/Login", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        MBTimeSheetWebApp.LoginService.LoginResponse Login(MBTimeSheetWebApp.LoginService.LoginRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="LoginService/Login", ReplyAction="*")]
        System.Threading.Tasks.Task<MBTimeSheetWebApp.LoginService.LoginResponse> LoginAsync(MBTimeSheetWebApp.LoginService.LoginRequest request);
        
        // CODEGEN: Generating message contract since the operation Logout is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action="LoginService/Logout", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        MBTimeSheetWebApp.LoginService.LogoutResponse Logout(MBTimeSheetWebApp.LoginService.LogoutRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="LoginService/Logout", ReplyAction="*")]
        System.Threading.Tasks.Task<MBTimeSheetWebApp.LoginService.LogoutResponse> LogoutAsync(MBTimeSheetWebApp.LoginService.LogoutRequest request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="LoginService")]
    public enum LoginDatabaseType {
        
        /// <remarks/>
        dst_MSSQL,
        
        /// <remarks/>
        dst_DB_2,
        
        /// <remarks/>
        dst_SYBASE,
        
        /// <remarks/>
        dst_MSSQL2005,
        
        /// <remarks/>
        dst_MSSQL2014,
        
        /// <remarks/>
        dst_MAXDB,
        
        /// <remarks/>
        dst_HANADB,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="LoginService")]
    public enum LoginLanguage {
        
        /// <remarks/>
        ln_Hebrew,
        
        /// <remarks/>
        ln_Spanish_Ar,
        
        /// <remarks/>
        ln_English,
        
        /// <remarks/>
        ln_Polish,
        
        /// <remarks/>
        ln_English_Sg,
        
        /// <remarks/>
        ln_Spanish_Pa,
        
        /// <remarks/>
        ln_English_Gb,
        
        /// <remarks/>
        ln_German,
        
        /// <remarks/>
        ln_Serbian,
        
        /// <remarks/>
        ln_Danish,
        
        /// <remarks/>
        ln_Norwegian,
        
        /// <remarks/>
        ln_Italian,
        
        /// <remarks/>
        ln_Hungarian,
        
        /// <remarks/>
        ln_Chinese,
        
        /// <remarks/>
        ln_Dutch,
        
        /// <remarks/>
        ln_Finnish,
        
        /// <remarks/>
        ln_Greek,
        
        /// <remarks/>
        ln_Portuguese,
        
        /// <remarks/>
        ln_Swedish,
        
        /// <remarks/>
        ln_English_Cy,
        
        /// <remarks/>
        ln_French,
        
        /// <remarks/>
        ln_Spanish,
        
        /// <remarks/>
        ln_Russian,
        
        /// <remarks/>
        ln_Spanish_La,
        
        /// <remarks/>
        ln_Czech_Cz,
        
        /// <remarks/>
        ln_Slovak_Sk,
        
        /// <remarks/>
        ln_Korean_Kr,
        
        /// <remarks/>
        ln_Portuguese_Br,
        
        /// <remarks/>
        ln_Japanese_Jp,
        
        /// <remarks/>
        ln_Turkish_Tr,
        
        /// <remarks/>
        ln_TrdtnlChinese_Hk,
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="Login", WrapperNamespace="LoginService", IsWrapped=true)]
    public partial class LoginRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="LoginService", Order=0)]
        public string DatabaseServer;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="LoginService", Order=1)]
        public string DatabaseName;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="LoginService", Order=2)]
        public MBTimeSheetWebApp.LoginService.LoginDatabaseType DatabaseType;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="LoginService", Order=3)]
        public string CompanyUsername;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="LoginService", Order=4)]
        public string CompanyPassword;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="LoginService", Order=5)]
        public MBTimeSheetWebApp.LoginService.LoginLanguage Language;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="LoginService", Order=6)]
        public string LicenseServer;
        
        public LoginRequest() {
        }
        
        public LoginRequest(string DatabaseServer, string DatabaseName, MBTimeSheetWebApp.LoginService.LoginDatabaseType DatabaseType, string CompanyUsername, string CompanyPassword, MBTimeSheetWebApp.LoginService.LoginLanguage Language, string LicenseServer) {
            this.DatabaseServer = DatabaseServer;
            this.DatabaseName = DatabaseName;
            this.DatabaseType = DatabaseType;
            this.CompanyUsername = CompanyUsername;
            this.CompanyPassword = CompanyPassword;
            this.Language = Language;
            this.LicenseServer = LicenseServer;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="LoginResponse", WrapperNamespace="http://www.sap.com/SBO/DIS", IsWrapped=true)]
    public partial class LoginResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.sap.com/SBO/DIS", Order=0)]
        public string SessionID;
        
        public LoginResponse() {
        }
        
        public LoginResponse(string SessionID) {
            this.SessionID = SessionID;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.sap.com/SBO/DIS")]
    public partial class MsgHeader : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string sessionIDField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string SessionID {
            get {
                return this.sessionIDField;
            }
            set {
                this.sessionIDField = value;
                this.RaisePropertyChanged("SessionID");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="LoginService")]
    public partial class Logout : object, System.ComponentModel.INotifyPropertyChanged {
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class LogoutRequest {
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://www.sap.com/SBO/DIS")]
        public MBTimeSheetWebApp.LoginService.MsgHeader MsgHeader;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="LoginService", Order=0)]
        public MBTimeSheetWebApp.LoginService.Logout Logout;
        
        public LogoutRequest() {
        }
        
        public LogoutRequest(MBTimeSheetWebApp.LoginService.MsgHeader MsgHeader, MBTimeSheetWebApp.LoginService.Logout Logout) {
            this.MsgHeader = MsgHeader;
            this.Logout = Logout;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="LogoutResponse", WrapperNamespace="http://www.sap.com/SBO/DIS", IsWrapped=true)]
    public partial class LogoutResponse {
        
        public LogoutResponse() {
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface LoginServiceSoapChannel : MBTimeSheetWebApp.LoginService.LoginServiceSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class LoginServiceSoapClient : System.ServiceModel.ClientBase<MBTimeSheetWebApp.LoginService.LoginServiceSoap>, MBTimeSheetWebApp.LoginService.LoginServiceSoap {
        
        public LoginServiceSoapClient() {
        }
        
        public LoginServiceSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public LoginServiceSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public LoginServiceSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public LoginServiceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        MBTimeSheetWebApp.LoginService.LoginResponse MBTimeSheetWebApp.LoginService.LoginServiceSoap.Login(MBTimeSheetWebApp.LoginService.LoginRequest request) {
            return base.Channel.Login(request);
        }
        
        public string Login(string DatabaseServer, string DatabaseName, MBTimeSheetWebApp.LoginService.LoginDatabaseType DatabaseType, string CompanyUsername, string CompanyPassword, MBTimeSheetWebApp.LoginService.LoginLanguage Language, string LicenseServer) {
            MBTimeSheetWebApp.LoginService.LoginRequest inValue = new MBTimeSheetWebApp.LoginService.LoginRequest();
            inValue.DatabaseServer = DatabaseServer;
            inValue.DatabaseName = DatabaseName;
            inValue.DatabaseType = DatabaseType;
            inValue.CompanyUsername = CompanyUsername;
            inValue.CompanyPassword = CompanyPassword;
            inValue.Language = Language;
            inValue.LicenseServer = LicenseServer;
            MBTimeSheetWebApp.LoginService.LoginResponse retVal = ((MBTimeSheetWebApp.LoginService.LoginServiceSoap)(this)).Login(inValue);
            return retVal.SessionID;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<MBTimeSheetWebApp.LoginService.LoginResponse> MBTimeSheetWebApp.LoginService.LoginServiceSoap.LoginAsync(MBTimeSheetWebApp.LoginService.LoginRequest request) {
            return base.Channel.LoginAsync(request);
        }
        
        public System.Threading.Tasks.Task<MBTimeSheetWebApp.LoginService.LoginResponse> LoginAsync(string DatabaseServer, string DatabaseName, MBTimeSheetWebApp.LoginService.LoginDatabaseType DatabaseType, string CompanyUsername, string CompanyPassword, MBTimeSheetWebApp.LoginService.LoginLanguage Language, string LicenseServer) {
            MBTimeSheetWebApp.LoginService.LoginRequest inValue = new MBTimeSheetWebApp.LoginService.LoginRequest();
            inValue.DatabaseServer = DatabaseServer;
            inValue.DatabaseName = DatabaseName;
            inValue.DatabaseType = DatabaseType;
            inValue.CompanyUsername = CompanyUsername;
            inValue.CompanyPassword = CompanyPassword;
            inValue.Language = Language;
            inValue.LicenseServer = LicenseServer;
            return ((MBTimeSheetWebApp.LoginService.LoginServiceSoap)(this)).LoginAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        MBTimeSheetWebApp.LoginService.LogoutResponse MBTimeSheetWebApp.LoginService.LoginServiceSoap.Logout(MBTimeSheetWebApp.LoginService.LogoutRequest request) {
            return base.Channel.Logout(request);
        }
        
        public void Logout(MBTimeSheetWebApp.LoginService.MsgHeader MsgHeader, MBTimeSheetWebApp.LoginService.Logout Logout1) {
            MBTimeSheetWebApp.LoginService.LogoutRequest inValue = new MBTimeSheetWebApp.LoginService.LogoutRequest();
            inValue.MsgHeader = MsgHeader;
            inValue.Logout = Logout1;
            MBTimeSheetWebApp.LoginService.LogoutResponse retVal = ((MBTimeSheetWebApp.LoginService.LoginServiceSoap)(this)).Logout(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<MBTimeSheetWebApp.LoginService.LogoutResponse> MBTimeSheetWebApp.LoginService.LoginServiceSoap.LogoutAsync(MBTimeSheetWebApp.LoginService.LogoutRequest request) {
            return base.Channel.LogoutAsync(request);
        }
        
        public System.Threading.Tasks.Task<MBTimeSheetWebApp.LoginService.LogoutResponse> LogoutAsync(MBTimeSheetWebApp.LoginService.MsgHeader MsgHeader, MBTimeSheetWebApp.LoginService.Logout Logout) {
            MBTimeSheetWebApp.LoginService.LogoutRequest inValue = new MBTimeSheetWebApp.LoginService.LogoutRequest();
            inValue.MsgHeader = MsgHeader;
            inValue.Logout = Logout;
            return ((MBTimeSheetWebApp.LoginService.LoginServiceSoap)(this)).LogoutAsync(inValue);
        }
    }
}
