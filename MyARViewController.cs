using Foundation;
using System;
using UIKit;
using Wikitude.Architect;
using CoreLocation;
using CoreMedia;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Dynamic;
using System.Web;


namespace IndoorNavigationProject1
{
	public partial class ArchitectViewDelegate : WTArchitectViewDelegate
	{
		public override void InvokedURL(WTArchitectView architectView, NSUrl url)
		{
			Console.WriteLine("architect view invoked url: " + url);
		}

		public override void DidFinishLoadNavigation(WTArchitectView architectView, WTNavigation navigation)
		{
			Console.WriteLine("architect view loaded navigation: " + navigation.OriginalURL);
		}

		public override void DidFailToLoadNavigation(WTArchitectView architectView, WTNavigation navigation, NSError error)
		{
			Console.WriteLine("architect view failed to load navigation. " + error.LocalizedDescription);
		}
	}


	public partial class MyARViewController : UIViewController
	{
		public string selectedBuilding;
		public string selectedFloor;
		public string selectedImpairment;
		public static string selectedPereference;
		public string selectedDestination;
		public string userSelection { get; set; }
		public List<Vertex> vertexes2 { get; set; }
		public Vertex end;
		public GenerateThePath generateThePath;
		protected string json;

		private WTArchitectView architectView;
		private WTAuthorizationRequestManager authorizationRequestManager = new WTAuthorizationRequestManager();
		private ArchitectViewDelegate architectViewDelegate = new ArchitectViewDelegate();

		private WTNavigation loadingArchitectWorldNavigation = null;

		private bool authorized = false;

		//public string WorldOrUrl { get; private set; }
		//public bool IsUrl { get; private set; }
		//public CLLocationManager _cllocaionManager;
		public MyARViewController(IntPtr handle) : base(handle)
		{
			
		}
		public MyARViewController(string userSelection, List<Vertex> vertexes2) : base()
		{
			userSelection = this.userSelection;
			vertexes2 = this.vertexes2;
		}

		public override void DidReceiveMemoryWarning()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning();

			// Release any cached data, images, etc that aren't in use.
		}

		#region View lifecycle

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			//add two help nodes to make sure the path does not go though the wall
			var helpNode_1 = new Vertex(1001, "helpNode_1", 9999, 2, "42.39177128009", "-72.5271138176322");
			var helpNode_2 = new Vertex(1002, "helpNode_2", 9999, 2, "42.39177128009", "-72.5270564854145");
			vertexes2.Add(helpNode_1);
			vertexes2.Add(helpNode_2);
      
      // **** value in this part is get from other viewcontroller
			string[] selection = userSelection.Split('@');
			selectedBuilding = selection[0];
			selectedFloor = selection[1];
			selectedImpairment = selection[2];
			selectedPereference = selection[3];
			selectedDestination = selection[4];
			foreach (var v in vertexes2)
			{
				var name = v.name;
				if (name == selectedDestination)
				{
					end = v;
				}
			}
      // *****
      
      // *** this part is create a path depends on a end point and a list of candidates point, the start point is fixed one
			generateThePath = new GenerateThePath();
			var path = generateThePath.loadThePath(end, vertexes2);
			json = JsonConvert.SerializeObject(selectedDestination);
      // *** for example the json = [{"name":"West Entrance","id":23,"distance":9999.0,"latitude":"42.3917031848521","longitude":"-72.5273525342345","floor":2},
                                     {"name":"helpNode_1","id":1001,"distance":9999.0,"latitude":"42.39177128009","longitude":"-72.5271138176322","floor":2},
                                     {"name":"helpNode_2","id":1002,"distance":9999.0,"latitude":"42.39177128009","longitude":"-72.5270564854145","floor":2},
                                     {"name":"Women's Restroom Main Floor","id":27,"distance":9999.0,"latitude":"42.3916895657956","longitude":"-72.5270045176148","floor":2}]
      
      
			this.architectView = new WTArchitectView();
			this.architectView.Delegate = architectViewDelegate;
			this.architectView.ShouldAuthorizeRestrictedAPIs = false;
			this.View.AddSubview(this.architectView);
			this.architectView.TranslatesAutoresizingMaskIntoConstraints = false;

			NSDictionary views = new NSDictionary(new NSString("architectView"), architectView);
			this.View.AddConstraints(NSLayoutConstraint.FromVisualFormat("|[architectView]|", 0, null, views));
			this.View.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|[architectView]|", 0, null, views));

			architectView.SetLicenseKey("0ZAMjtemDOyxWRVgwCi2YwN7dxlgVCH1cXF6tc5ft5CoqD2hn/TwZU200zp6D92AJml2cWMQu4HoFfF/g7+Ramu51JAFBTcpoSh+8GoocbN2BBaHl+VGva4vTsc/hUiVaCYAZQvR8P8Rvvzg/0Y4NIuoUUu4SFtdyWc4FZQ5lnBTYWx0ZWRfX0/0PMFo5mnbAx8QRHLauV61W7oK1vEhNo0b0mgxi8TvH+k33FSMMvViqPnYJ2Os7KZ1QkBRWhMjDLV+ASVB+6FLBdJz2T2cQZS7aQTQdoD3HiW7i93M0GolE9rOTmMJFI0gDfu6R9q865c47LbiN/98ZpHsVRKUXEaecJgrdeEum5VqkvH+YSfN7Xa5R1bz7Jf20IbopTiqvlel6DApqnm8Xg1L8iO95wsOysYiNaZt5mnH26ZZkI/24dc87VDR+CSALPzmn5cFBVzq5wcNMF6QciRw+CcrdY90cL6lqRIz0PSIEG5nOFXAUhEjZxmKAMjP9fWEOfuJVOUqa7ssW5dKMU4sNuR0yFdtJkK9f93PzFdK7V5l3+195rdOg6gsgrPhlK7eScssoD7GdK0HNPFeD4htocS3/meNb2nv0kJG3twtAiDJaggqy9QKZVcDJrs9flhndCBrUDB1u3u2+XPw8SmfjD9v/csH5njefAvLO8YsOQDIKAc8JyajWn3NvqUwJfaLtBIgg2Du1AiTGvtsdNhqz/Cyvw==");

			NSNotificationCenter.DefaultCenter.AddObserver(UIApplication.DidBecomeActiveNotification, (notification) =>
			{
				StartAR();
			});

			NSNotificationCenter.DefaultCenter.AddObserver(UIApplication.WillResignActiveNotification, (notification) =>
			{
				StopAR();
			});

		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);

			if (!authorizationRequestManager.RequestingRestrictedAppleiOSSDKAPIAuthorization)
			{
				NSOrderedSet<NSNumber> restrictedAppleiOSSKDAPIs = WTAuthorizationRequestManager.RestrictedAppleiOSSDKAPIAuthorizationsForRequiredFeatures(WTFeatures.WTFeature_ImageTracking);
				authorizationRequestManager.RequestRestrictedAppleiOSSDKAPIAuthorization(restrictedAppleiOSSKDAPIs, (bool success, NSError error) =>
				{
					authorized = success;
					if (success)
					{
						StartAR();
					}
					else
					{
						handleAuthorizationError(error);
					}
				});
			}
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);
		}

		public override void ViewDidDisappear(bool animated)
		{
			base.ViewDidDisappear(animated);

			StopAR();
		}

		#endregion

		#region Rotation

		public override void WillRotate(UIInterfaceOrientation toInterfaceOrientation, double duration)
		{
			base.WillRotate(toInterfaceOrientation, duration);

			architectView.SetShouldRotateToInterfaceOrientation(true, toInterfaceOrientation);
		}

		public override bool ShouldAutorotate()
		{
			return true;
		}

		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
		{
			return UIInterfaceOrientationMask.All;
		}

		#endregion

		#region Segue
		[Action("UnwindToMainViewController")]
		public void UnwindToMainViewController()
		{
		}
		#endregion

		#region Private Methods
		private void handleAuthorizationError(NSError authorizationError)
		{
			NSDictionary unauthorizedAPIInfo = (NSDictionary)authorizationError.UserInfo.ObjectForKey(WTAuthorizationRequestManager.WTUnauthorizedAppleiOSSDKAPIsKey);

			NSMutableString detailedAuthorizationErrorLogMessage = (NSMutableString)new NSString("The following authorization states do not meet the requirements:").MutableCopy();
			NSMutableString missingAuthorizations = (NSMutableString)new NSString("In order to use the Wikitude SDK, please grant access to the following:").MutableCopy();
			foreach (NSString unauthorizedAPIKey in unauthorizedAPIInfo.Keys)
			{
				NSNumber unauthorizedAPIValue = (NSNumber)unauthorizedAPIInfo.ObjectForKey(unauthorizedAPIKey);
				detailedAuthorizationErrorLogMessage.Append(new NSString("\n"));
				detailedAuthorizationErrorLogMessage.Append(unauthorizedAPIKey);
				detailedAuthorizationErrorLogMessage.Append(new NSString(" = "));
				detailedAuthorizationErrorLogMessage.Append(WTAuthorizationRequestManager.StringFromAuthorizationStatusForUnauthorizedAppleiOSSDKAPI(unauthorizedAPIValue.Int32Value, unauthorizedAPIKey));

				missingAuthorizations.Append(new NSString("\n *"));
				missingAuthorizations.Append(WTAuthorizationRequestManager.HumanReadableDescriptionForUnauthorizedAppleiOSSDKAPI(unauthorizedAPIKey));
			}
			Console.WriteLine(detailedAuthorizationErrorLogMessage);

			UIAlertController settingsAlertController = UIAlertController.Create("Required API authorizations missing", missingAuthorizations, UIAlertControllerStyle.Alert);
			settingsAlertController.AddAction(UIAlertAction.Create("Open Settings", UIAlertActionStyle.Default, (UIAlertAction obj) =>
			{
				UIApplication.SharedApplication.OpenUrl(new NSUrl(UIApplication.OpenSettingsUrlString));
			}));
			settingsAlertController.AddAction(UIAlertAction.Create("NO", UIAlertActionStyle.Destructive, (UIAlertAction obj) => { }));
			this.PresentViewController(settingsAlertController, true, null);
		}

		private void StartAR()
		{
			if (authorized)
			{
				if (!architectView.IsRunning)
				{
					architectView.Start((startupConfiguration) =>
					{
						// use startupConfiguration.CaptureDevicePosition = AVFoundation.AVCaptureDevicePosition.Front; to start the Wikitude SDK with an active front cam
						startupConfiguration.CaptureDevicePosition = AVFoundation.AVCaptureDevicePosition.Back;

						startupConfiguration.CaptureDeviceResolution = WTCaptureDeviceResolution.WTCaptureDeviceResolution_AUTO;
						startupConfiguration.TargetFrameRate = CMTime.PositiveInfinity; // resolves to WTMakeTargetFrameRateAuto();
					}, (bool isRunning, NSError startupError) =>
					{
						if (isRunning)
						{
							if (null == loadingArchitectWorldNavigation)
							{
								var path = NSBundle.MainBundle.BundleUrl.AbsoluteString + "07_ObtainPoiData_1_FromApplicationModel/index.html";
								loadingArchitectWorldNavigation = architectView.LoadArchitectWorldFromURL(NSUrl.FromString(path), Wikitude.Architect.WTFeatures.WTFeature_ImageTracking);
								//this.architectView.CallJavaScript("passData(" + json +")");
                this.architectView.CallJavaScript("World.loadPoisFromJsonData(" + json + ");");
							}

						}
						else
						{
							Console.WriteLine("Unable to start Wikitude SDK. Error (start ar): " + startupError.LocalizedDescription);
						}
					});
				}

				architectView.SetShouldRotateToInterfaceOrientation(true, UIApplication.SharedApplication.StatusBarOrientation);
			}
		}

		private void StopAR()
		{
			if (architectView.IsRunning)
			{
				architectView.Stop();
			}
		}
		#endregion
	}
}
