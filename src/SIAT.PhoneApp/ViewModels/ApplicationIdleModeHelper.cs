using System;
using System.ComponentModel;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Controls;

namespace SIAT.PhoneApp.ViewModels
{
    public class ApplicationIdleModeHelper : INotifyPropertyChanged 
    {
        private ApplicationIdleModeHelper()
        {
        }

        static ApplicationIdleModeHelper _current;

        public static ApplicationIdleModeHelper Current
        {
            get
            {
                if (null == _current)
                {
                    _current = new ApplicationIdleModeHelper(); 
                    _current.Initialize();
                }
                System.Diagnostics.Debug.Assert(_current != null); 

                return _current; 
            }
        }

    
        private void Initialize() 
        {
            System.Diagnostics.Debug.WriteLine("initialized " + PhoneApplicationService.Current.StartupMode );

            // Load RunUnderLock from Storage
            bool fromStorage = false;
            if (IsolatedStorageSettings.ApplicationSettings.TryGetValue("RunsUnderLock", out fromStorage))
            {
                if (fromStorage != (PhoneApplicationService.Current.ApplicationIdleDetectionMode == IdleDetectionMode.Disabled))
                {
                    RunsUnderLock = fromStorage;
                    System.Diagnostics.Debug.WriteLine("Did not match");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Matched"); 
                    runsUnderLock = fromStorage;
                } 
            }

            // Load PreventLock from Storage
            if (IsolatedStorageSettings.ApplicationSettings.TryGetValue("PreventLock", out fromStorage))
            {
                if (fromStorage != (PhoneApplicationService.Current.UserIdleDetectionMode == IdleDetectionMode.Disabled))
                {
                    PreventLock = fromStorage;
                    System.Diagnostics.Debug.WriteLine("Did not match");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Matched");
                    preventScreenToLock = fromStorage;
                }
            }

            bool hasPrompted = false;
            if (IsolatedStorageSettings.ApplicationSettings.TryGetValue("HasUserAgreedToRunUnderLock", out hasPrompted))
            {
                HasUserAgreedToRunUnderLock = hasPrompted;
            }    
         
            
        } 


#region private members 
        bool runsUnderLock; 
        bool isRunningUnderLock;
        bool hasUserAgreedToRunUnderLock;
        bool isRestartRequired;
        bool preventScreenToLock;

#endregion 

#region public properties 
        public bool RunsUnderLock
        {
            get
            {
                return runsUnderLock; 
            } 
            set 
            {                
                if ( value != runsUnderLock ) 
                { 
                    runsUnderLock = value ; 


                    if ( runsUnderLock ) 
                    { 
                        PhoneApplicationService.Current.ApplicationIdleDetectionMode = IdleDetectionMode.Disabled;
                        PhoneApplicationFrame rootframe = App.Current.RootVisual as PhoneApplicationFrame ;

                        System.Diagnostics.Debug.Assert(rootframe != null, "This sample assumes RootVisual has been set"); 
                        if (rootframe != null)
                        {
                            rootframe.Obscured += new EventHandler<ObscuredEventArgs>(rootframe_Obscured);
                            rootframe.Unobscured += new EventHandler(rootframe_Unobscured);
                        }
                         
                    } 
                    else 
                    {
                        IsRestartRequired = true;
                        // we can not set it; we have to restart app ...
                        //  PhoneApplicationService.Current.ApplicationIdleDetectionMode = IdleDetectionMode.Enabled ; 
                        EventHandler eh = RestartRequired;
                        if (eh != null)
                            eh(this, new EventArgs()); 
                    }

                    ApplicationPhoneSettings.SaveSetting("RunsUnderLock", runsUnderLock); 
                    OnPropertyChanged("RunsUnderLock"); 
                } 
            } 
        }

        public bool PreventLock
        {
            get
            {
                return preventScreenToLock;
            }
            set
            {
                if (value != preventScreenToLock)
                {
                    preventScreenToLock = value;

                    if (preventScreenToLock)
                    {
                        PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
                    }
                    else
                    {
                        PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Enabled;
                    }

                    ApplicationPhoneSettings.SaveSetting("PreventLock", preventScreenToLock);
                    OnPropertyChanged("PreventLock");
                }
            }
        }


        public bool IsRestartRequired
        {
            get
            {
                return isRestartRequired; 
            }
            private set
            {
                if (value != isRestartRequired)
                {
                    isRestartRequired = value;
                    OnPropertyChanged("IsRestartRequired"); 
                } 
            } 
        } 

        public bool HasUserAgreedToRunUnderLock
        {
            get
            {
                return hasUserAgreedToRunUnderLock; 
            }
            set
            {
                if (value != hasUserAgreedToRunUnderLock)
                {
                    hasUserAgreedToRunUnderLock = value;
                    ApplicationPhoneSettings.SaveSetting("HasUserAgreedToRunUnderLock", hasUserAgreedToRunUnderLock);
                    OnPropertyChanged("HasUserAgreedToRunUnderLock"); 
                } 
            }
        }

         

        void  rootframe_Unobscured(object sender, EventArgs e)
        {   
 	        IsRunningUnderLock = false;  
            
        }

        void  rootframe_Obscured(object sender, ObscuredEventArgs e)
        {
 	        if (e.IsLocked ) 
            { 
                IsRunningUnderLock = e.IsLocked ; 
            } 
        }

        public bool IsRunningUnderLock 
        {
            get
            {
                return isRunningUnderLock;  
            }
            private set 
            { 
                 if ( value != isRunningUnderLock )
                 { 
                    isRunningUnderLock = value ;                    
                    OnPropertyChanged("IsRunningUnderLock") ;

                    if (isRunningUnderLock)
                    {
                        EventHandler eh = Locked;
                        if (eh != null)
                            eh(this, new EventArgs());
                    }
                    else
                    {
                        EventHandler eh = UnLocked;
                        if (eh != null)
                            eh(this, new EventArgs());
                    } 

                 } 

            } 
        } 

#endregion 

#region events 

        public event EventHandler Locked ;
        public event EventHandler UnLocked;
        public event EventHandler RestartRequired; 

#endregion 


#region INotifyPropertyChanged 

    public event PropertyChangedEventHandler  PropertyChanged;

    private void OnPropertyChanged ( string propertyName ) 
    { 
       PropertyChangedEventHandler ph = PropertyChanged; 
        if ( ph != null )
            ph ( this, new PropertyChangedEventArgs ( propertyName ));  

    } 

#endregion 

    

}
}
