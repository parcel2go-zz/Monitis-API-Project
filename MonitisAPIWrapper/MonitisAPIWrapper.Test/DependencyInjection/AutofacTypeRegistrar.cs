///    copyright parcel2go.com 2011 www.parcel2go.com
///
///    This program is free software: you can redistribute it and/or modify
///    it under the terms of the GNU General Public License as published by
///    the Free Software Foundation, either version 3 of the License, or
///    (at your option) any later version.

///    This program is distributed in the hope that it will be useful,
///    but WITHOUT ANY WARRANTY; without even the implied warranty of
///    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
///    GNU General Public License for more details.

///    You should have received a copy of the GNU General Public License
///    along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System.Collections.Generic;
using Autofac;
using MonitisAPIWrapper.Helper;
using Common.Logging;

// custom monitors
using MonitisAPIWrapper.CustomMonitors.Get;
using MonitisAPIWrapper.CustomMonitors.Post;
using MonitisAPIWrapper.CustomMonitors.Responses;
using MonitisAPIWrapper.CustomMonitors.Responses.MonitorInfo;

// external monitors 
using MonitisAPIWrapper.PredefinedMonitors.ExternalMonitors.Post;
using MonitisAPIWrapper.PredefinedMonitors.ExternalMonitors.Responses;
using MonitisAPIWrapper.PredefinedMonitors.ExternalMonitors.Get;

// full page monitors
using MonitisAPIWrapper.PredefinedMonitors.FullPageLoadMonitors.Post;
using MonitisAPIWrapper.PredefinedMonitors.FullPageLoadMonitors.Responses;

// transaction monitors
using MonitisAPIWrapper.PredefinedMonitors.TransactionMonitors.Response;
using MonitisAPIWrapper.PredefinedMonitors.TransactionMonitors.Post;
using MonitisAPIWrapper.PredefinedMonitors.TransactionMonitors.Get;

namespace MonitisAPIWrapper.Test.DependencyInjection
{
    public class AutofacTypeRegistrar : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterApplicationSpecificTypes(builder);
        }

        private static void RegisterApplicationSpecificTypes(ContainerBuilder builder)
        {
            const string API_KEY = ""; // your secret key here
            const string SECRET_KEY = ""; // your password here
            const int API_VERSION = 2;
            const string CUSTOM_MONITOR_API_URL = "http://monitis.com/customMonitorApi";
            const string EXTERNAL_MONITOR_API_URL = "http://monitis.com/api";

            builder.Register(c => new ChecksumCalculator(SECRET_KEY));
            builder.Register(c => new AuthenticationTokenGetter<AuthenticationTokenResponse>(API_KEY, SECRET_KEY, API_VERSION, LogManager.GetLogger(typeof (AuthenticationTokenGetter <AuthenticationTokenResponse>))));
            
            // custom monitor requests
            builder.Register<IRequestSender<SimpleResponse>>(c => new PostRequestSender<SimpleResponse>(CUSTOM_MONITOR_API_URL, "addMonitor", API_KEY, c.Resolve<ChecksumCalculator>(), c.Resolve<AuthenticationTokenGetter<AuthenticationTokenResponse>>(), API_VERSION, LogManager.GetLogger(typeof(PostRequestSender<SimpleResponse>)))).Named<IRequestSender<SimpleResponse>>("AddCustomMonitor");
            builder.Register<IRequestSender<SimpleResponse>>(c => new PostRequestSender<SimpleResponse>(CUSTOM_MONITOR_API_URL, "addResult", API_KEY, c.Resolve<ChecksumCalculator>(), c.Resolve<AuthenticationTokenGetter<AuthenticationTokenResponse>>(), API_VERSION, LogManager.GetLogger(typeof(PostRequestSender<SimpleResponse>)))).Named<IRequestSender<SimpleResponse>>("AddCustomResult");
            builder.Register<IRequestSender<SimpleResponse>>(c => new PostRequestSender<SimpleResponse>(CUSTOM_MONITOR_API_URL, "deleteMonitor", API_KEY, c.Resolve<ChecksumCalculator>(), c.Resolve<AuthenticationTokenGetter<AuthenticationTokenResponse>>(), API_VERSION, LogManager.GetLogger(typeof(PostRequestSender<SimpleResponse>)))).Named<IRequestSender<SimpleResponse>>("DeleteCustomMonitor");
            builder.Register<IRequestSender<SimpleResponse>>(c => new PostRequestSender<SimpleResponse>(CUSTOM_MONITOR_API_URL, "editMonitor", API_KEY, c.Resolve<ChecksumCalculator>(), c.Resolve<AuthenticationTokenGetter<AuthenticationTokenResponse>>(), API_VERSION, LogManager.GetLogger(typeof(PostRequestSender<SimpleResponse>)))).Named<IRequestSender<SimpleResponse>>("EditCustomMonitor");
            builder.Register<IRequestSender<SimpleResponse>>(c => new PostRequestSender<SimpleResponse>(CUSTOM_MONITOR_API_URL, "addAdditionalResults", API_KEY, c.Resolve<ChecksumCalculator>(), c.Resolve<AuthenticationTokenGetter<AuthenticationTokenResponse>>(), API_VERSION, LogManager.GetLogger(typeof(PostRequestSender<SimpleResponse>)))).Named<IRequestSender<SimpleResponse>>("AddAdditionalCustomResult");
           
            builder.Register<IRequestSender<List<CustomMonitorListResponse>>>(c => new GetRequestSender<List<CustomMonitorListResponse>>(CUSTOM_MONITOR_API_URL, "getMonitors", API_KEY, c.Resolve<ChecksumCalculator>(), c.Resolve<AuthenticationTokenGetter<AuthenticationTokenResponse>>(), API_VERSION, LogManager.GetLogger(typeof(GetRequestSender<List<CustomMonitorListResponse>>)))).Named<IRequestSender<List<CustomMonitorListResponse>>>("GetCustomMonitors");
            builder.Register<IRequestSender<CustomMonitorInfoResponse>>(c => new GetRequestSender<CustomMonitorInfoResponse>(CUSTOM_MONITOR_API_URL, "getMonitorInfo", API_KEY, c.Resolve<ChecksumCalculator>(), c.Resolve<AuthenticationTokenGetter<AuthenticationTokenResponse>>(), API_VERSION, LogManager.GetLogger(typeof(GetRequestSender<CustomMonitorInfoResponse>)))).Named<IRequestSender<CustomMonitorInfoResponse>>("GetCustomMonitorInfo");
            builder.Register<IRequestSender<List<CustomMonitorResultResponse>>>(c => new GetRequestSender<List<CustomMonitorResultResponse>>(CUSTOM_MONITOR_API_URL, "getMonitorResults", API_KEY, c.Resolve<ChecksumCalculator>(), c.Resolve<AuthenticationTokenGetter<AuthenticationTokenResponse>>(), API_VERSION, LogManager.GetLogger(typeof(GetRequestSender<List<CustomMonitorResultResponse>>)))).Named<IRequestSender<List<CustomMonitorResultResponse>>>("GetCustomMonitorResults");
            builder.Register<IRequestSender<List<MonitorAdditionalResultResponse>>>(c => new GetRequestSender<List<MonitorAdditionalResultResponse>>(CUSTOM_MONITOR_API_URL, "getAdditionalResults", API_KEY, c.Resolve<ChecksumCalculator>(), c.Resolve<AuthenticationTokenGetter<AuthenticationTokenResponse>>(), API_VERSION, LogManager.GetLogger(typeof(GetRequestSender<List<MonitorAdditionalResultResponse>>)))).Named<IRequestSender<List<MonitorAdditionalResultResponse>>>("GetAdditionalCustomMonitorResults");
            // custom monitor requests 

            // custom monitor classes
            builder.Register(c => new CustomMonitorAdder(c.Resolve<IRequestSender<SimpleResponse>>("AddCustomMonitor"), LogManager.GetLogger(typeof(CustomMonitorAdder))));
            builder.Register(c => new CustomMonitorResultsAdder(c.Resolve<IRequestSender<SimpleResponse>>("AddCustomResult"), LogManager.GetLogger(typeof(CustomMonitorResultsAdder))));
            builder.Register(c => new CustomMonitorDeleter(c.Resolve<IRequestSender<SimpleResponse>>("DeleteCustomMonitor"), LogManager.GetLogger(typeof(CustomMonitorDeleter))));
            builder.Register(c => new CustomMonitorEditer(c.Resolve<IRequestSender<SimpleResponse>>("EditCustomMonitor"), LogManager.GetLogger(typeof (CustomMonitorEditer))));
            builder.Register(c => new AdditonalCustomMonitorResultsAdder(c.Resolve<IRequestSender<SimpleResponse>>("AddAdditionalCustomResult"), LogManager.GetLogger(typeof (AdditonalCustomMonitorResultsAdder))));

            builder.Register(c => new AdditionalCustomMonitorResultsGetter(c.Resolve<IRequestSender<List<MonitorAdditionalResultResponse>>>("GetAdditionalCustomMonitorResults"), LogManager.GetLogger(typeof(AdditionalCustomMonitorResultsGetter))));            
            builder.Register(c => new CustomMonitorResultsGetter(c.Resolve<IRequestSender<List<CustomMonitorResultResponse>>>("GetCustomMonitorResults"), LogManager.GetLogger(typeof(CustomMonitors.Get.CustomMonitorResultsGetter))));
            builder.Register(c => new CustomMonitorLister(c.Resolve<IRequestSender<List<CustomMonitorListResponse>>>("GetCustomMonitors"), LogManager.GetLogger(typeof(CustomMonitorLister))));
            builder.Register(c => new CustomMonitorInfoGetter(c.Resolve<IRequestSender<CustomMonitorInfoResponse>>("GetCustomMonitorInfo"), LogManager.GetLogger(typeof(CustomMonitorInfoGetter))));
            // custom monitor classes

            // external monitor requests
            builder.Register<IRequestSender<SimpleResponse>>(c => new PostRequestSender<SimpleResponse>(EXTERNAL_MONITOR_API_URL, "activateExternalMonitor", API_KEY, c.Resolve<ChecksumCalculator>(), c.Resolve<AuthenticationTokenGetter<AuthenticationTokenResponse>>(), API_VERSION, LogManager.GetLogger(typeof (PostRequestSender<SimpleResponse>)))).Named<IRequestSender<SimpleResponse>>("ActivateExternalMonitor");
            builder.Register<IRequestSender<SimpleResponse>>(c => new PostRequestSender<SimpleResponse>(EXTERNAL_MONITOR_API_URL, "addExternalMonitor", API_KEY, c.Resolve<ChecksumCalculator>(), c.Resolve<AuthenticationTokenGetter<AuthenticationTokenResponse>>(), API_VERSION, LogManager.GetLogger(typeof(PostRequestSender<SimpleResponse>)))).Named<IRequestSender<SimpleResponse>>("AddExternalMonitor");
            builder.Register<IRequestSender<SimpleResponse>>(c => new PostRequestSender<SimpleResponse>(EXTERNAL_MONITOR_API_URL, "deleteExternalMonitor", API_KEY, c.Resolve<ChecksumCalculator>(), c.Resolve<AuthenticationTokenGetter<AuthenticationTokenResponse>>(), API_VERSION, LogManager.GetLogger(typeof(PostRequestSender<SimpleResponse>)))).Named<IRequestSender<SimpleResponse>>("DeleteExternalMonitor");
            builder.Register<IRequestSender<SimpleResponse>>(c => new PostRequestSender<SimpleResponse>(EXTERNAL_MONITOR_API_URL, "editExternalMonitor", API_KEY, c.Resolve<ChecksumCalculator>(), c.Resolve<AuthenticationTokenGetter<AuthenticationTokenResponse>>(), API_VERSION, LogManager.GetLogger(typeof(PostRequestSender<SimpleResponse>)))).Named<IRequestSender<SimpleResponse>>("EditExternalMonitor");
            builder.Register<IRequestSender<SimpleResponse>>(c => new PostRequestSender<SimpleResponse>(EXTERNAL_MONITOR_API_URL, "suspendExternalMonitor", API_KEY, c.Resolve<ChecksumCalculator>(), c.Resolve<AuthenticationTokenGetter<AuthenticationTokenResponse>>(), API_VERSION, LogManager.GetLogger(typeof(PostRequestSender<SimpleResponse>)))).Named<IRequestSender<SimpleResponse>>("SuspendExternalMonitor");

            builder.Register<IRequestSender<List<LocationsResponse>>>(c => new GetRequestSender<List<LocationsResponse>>(EXTERNAL_MONITOR_API_URL, "locations", API_KEY, c.Resolve<ChecksumCalculator>(), c.Resolve<AuthenticationTokenGetter<AuthenticationTokenResponse>>(), API_VERSION, LogManager.GetLogger(typeof(GetRequestSender<List<LocationsResponse>>)))).Named<IRequestSender<List<LocationsResponse>>>("GetLocations");
            builder.Register<IRequestSender<ExternalMonitorInfoResponse>>(c => new GetRequestSender<ExternalMonitorInfoResponse>(EXTERNAL_MONITOR_API_URL, "testinfo", API_KEY, c.Resolve<ChecksumCalculator>(), c.Resolve<AuthenticationTokenGetter<AuthenticationTokenResponse>>(), API_VERSION, LogManager.GetLogger(typeof(GetRequestSender<ExternalMonitorInfoResponse>)))).Named<IRequestSender<ExternalMonitorInfoResponse>>("GetExternalMonitorInfo");
            builder.Register<IRequestSender<List<ExternalMonitorResultsResponse>>>(c => new GetRequestSender<List<ExternalMonitorResultsResponse>>(EXTERNAL_MONITOR_API_URL, "testresult", API_KEY, c.Resolve<ChecksumCalculator>(), c.Resolve<AuthenticationTokenGetter<AuthenticationTokenResponse>>(), API_VERSION, LogManager.GetLogger(typeof(GetRequestSender<List<ExternalMonitorResultsResponse>>)))).Named<IRequestSender<List<ExternalMonitorResultsResponse>>>("GetExternalMonitorResults");
            builder.Register<IRequestSender<ExternalMonitorsByTagResponse>>(c => new GetRequestSender<ExternalMonitorsByTagResponse>(EXTERNAL_MONITOR_API_URL, "tagtests", API_KEY, c.Resolve<ChecksumCalculator>(), c.Resolve<AuthenticationTokenGetter<AuthenticationTokenResponse>>(), API_VERSION, LogManager.GetLogger(typeof(GetRequestSender<ExternalMonitorsByTagResponse>)))).Named<IRequestSender<ExternalMonitorsByTagResponse>>("GetExternalMonitorByTag");
            builder.Register<IRequestSender<ExternalMonitorsResponse>>(c => new GetRequestSender<ExternalMonitorsResponse>(EXTERNAL_MONITOR_API_URL, "tests", API_KEY, c.Resolve<ChecksumCalculator>(), c.Resolve<AuthenticationTokenGetter<AuthenticationTokenResponse>>(), API_VERSION, LogManager.GetLogger(typeof(GetRequestSender<ExternalMonitorsResponse>)))).Named<IRequestSender<ExternalMonitorsResponse>>("GetExternalMonitors");
            builder.Register<IRequestSender<List<SnapshotResponse>>>(c => new GetRequestSender<List<SnapshotResponse>>(EXTERNAL_MONITOR_API_URL, "testsLastValues", API_KEY, c.Resolve<ChecksumCalculator>(), c.Resolve<AuthenticationTokenGetter<AuthenticationTokenResponse>>(), API_VERSION, LogManager.GetLogger(typeof(GetRequestSender<List<SnapshotResponse>>)))).Named<IRequestSender<List<SnapshotResponse>>>("GetSnapshot");
            builder.Register<IRequestSender<TagsResponse>>(c => new GetRequestSender<TagsResponse>(EXTERNAL_MONITOR_API_URL, "tags", API_KEY, c.Resolve<ChecksumCalculator>(), c.Resolve<AuthenticationTokenGetter<AuthenticationTokenResponse>>(), API_VERSION, LogManager.GetLogger(typeof(GetRequestSender<TagsResponse>)))).Named<IRequestSender<TagsResponse>>("GetTags");
            // external monitor requests

            // external monitor classes
            builder.Register(c => new ExternalMonitorActivator(c.Resolve<IRequestSender<SimpleResponse>>("ActivateExternalMonitor"), LogManager.GetLogger(typeof (ExternalMonitorActivator))));
            builder.Register(c => new ExternalMonitorAdder(c.Resolve<IRequestSender<SimpleResponse>>("AddExternalMonitor"), LogManager.GetLogger(typeof(ExternalMonitorAdder))));
            builder.Register(c => new ExternalMonitorDeleter(c.Resolve<IRequestSender<SimpleResponse>>("DeleteExternalMonitor"), LogManager.GetLogger(typeof(ExternalMonitorDeleter))));
            builder.Register(c => new ExternalMonitorEditer(c.Resolve<IRequestSender<SimpleResponse>>("EditExternalMonitor"), LogManager.GetLogger(typeof(ExternalMonitorEditer))));
            builder.Register(c => new ExternalMonitorSuspender(c.Resolve<IRequestSender<SimpleResponse>>("SuspendExternalMonitor"), LogManager.GetLogger(typeof(ExternalMonitorSuspender))));

            builder.Register(c => new LocationsGetter(c.Resolve<IRequestSender<List<LocationsResponse>>>("GetLocations"), LogManager.GetLogger(typeof(LocationsGetter))));
            builder.Register(c => new ExternalMonitorInfoGetter(c.Resolve<IRequestSender<ExternalMonitorInfoResponse>>("GetExternalMonitorInfo"), LogManager.GetLogger(typeof(ExternalMonitorInfoGetter))));
            builder.Register(c => new ExternalMonitorResultsGetter(c.Resolve<IRequestSender<List<ExternalMonitorResultsResponse>>>("GetExternalMonitorResults"), LogManager.GetLogger(typeof(ExternalMonitorResultsGetter))));
            builder.Register(c => new ExternalMonitorsByTagGetter(c.Resolve<IRequestSender<ExternalMonitorsByTagResponse>>("GetExternalMonitorByTag"), LogManager.GetLogger(typeof(ExternalMonitorsByTagGetter))));
            builder.Register(c => new ExternalMonitorsGetter(c.Resolve<IRequestSender<ExternalMonitorsResponse>>("GetExternalMonitors"), LogManager.GetLogger(typeof(ExternalMonitorsGetter))));
            builder.Register(c => new SnapshotGetter(c.Resolve<IRequestSender<List<SnapshotResponse>>>("GetSnapshot"), LogManager.GetLogger(typeof(SnapshotGetter))));
            builder.Register(c => new TagsGetter(c.Resolve<IRequestSender<TagsResponse>>("GetTags"), LogManager.GetLogger(typeof(TagsGetter))));
            // external monitor classes

            // full page load requests
            builder.Register<IRequestSender<SimpleResponse>>(c => new PostRequestSender<SimpleResponse>(EXTERNAL_MONITOR_API_URL, "activateFullPageLoadMonitor", API_KEY, c.Resolve<ChecksumCalculator>(), c.Resolve<AuthenticationTokenGetter<AuthenticationTokenResponse>>(), API_VERSION, LogManager.GetLogger(typeof(PostRequestSender<SimpleResponse>)))).Named<IRequestSender<SimpleResponse>>("ActivateFullPageLoadMonitor");
            builder.Register<IRequestSender<SimpleResponse>>(c =>new PostRequestSender<SimpleResponse>(EXTERNAL_MONITOR_API_URL, "addFullPageLoadMonitor", API_KEY, c.Resolve<ChecksumCalculator>(), c.Resolve<AuthenticationTokenGetter<AuthenticationTokenResponse>>(), API_VERSION, LogManager.GetLogger(typeof (PostRequestSender<SimpleResponse>)))).Named<IRequestSender<SimpleResponse>>("AddFullPageLoadMonitor");
            builder.Register<IRequestSender<SimpleResponse>>(c => new PostRequestSender<SimpleResponse>(EXTERNAL_MONITOR_API_URL, "editFullPageLoadMonitor", API_KEY, c.Resolve<ChecksumCalculator>(), c.Resolve<AuthenticationTokenGetter<AuthenticationTokenResponse>>(), API_VERSION, LogManager.GetLogger(typeof(PostRequestSender<SimpleResponse>)))).Named<IRequestSender<SimpleResponse>>("EditFullPageLoadMonitor");
            builder.Register<IRequestSender<FullPageMonitorSuspendResponse>>(c => new PostRequestSender<FullPageMonitorSuspendResponse>(EXTERNAL_MONITOR_API_URL, "suspendFullPageLoadMonitor", API_KEY, c.Resolve<ChecksumCalculator>(), c.Resolve<AuthenticationTokenGetter<AuthenticationTokenResponse>>(), API_VERSION, LogManager.GetLogger(typeof(PostRequestSender<FullPageMonitorSuspendResponse>)))).Named<IRequestSender<FullPageMonitorSuspendResponse>>("SuspendFullPageLoadMonitor");
            // full page load requests

            // full page load classes
            builder.Register(c => new FullPageMonitorActivator(c.Resolve<IRequestSender<SimpleResponse>>("ActivateFullPageLoadMonitor"), LogManager.GetLogger(typeof (FullPageMonitorActivator))));
            builder.Register(c => new FullPageMonitorAdder(c.Resolve<IRequestSender<SimpleResponse>>("AddFullPageLoadMonitor"), LogManager.GetLogger(typeof(FullPageMonitorAdder))));
            builder.Register(c => new FullPageMonitorEditer(c.Resolve<IRequestSender<SimpleResponse>>("EditFullPageLoadMonitor"), LogManager.GetLogger(typeof(FullPageMonitorEditer))));
            builder.Register(c => new FullPageMonitorSuspender(c.Resolve<IRequestSender<FullPageMonitorSuspendResponse>>("SuspendFullPageLoadMonitor"), LogManager.GetLogger(typeof(FullPageMonitorSuspender))));
            // full page load classes

            // transaction monitor requests
            builder.Register<IRequestSender<TransactionMonitorSuspendResponse>>(c => new PostRequestSender<TransactionMonitorSuspendResponse>(EXTERNAL_MONITOR_API_URL, "suspendTransactionMonitor", API_KEY, c.Resolve<ChecksumCalculator>(), c.Resolve<AuthenticationTokenGetter<AuthenticationTokenResponse>>(), API_VERSION, LogManager.GetLogger(typeof(PostRequestSender<TransactionMonitorSuspendResponse>)))).Named<IRequestSender<TransactionMonitorSuspendResponse>>("SuspendTransactionMonitor");
            builder.Register<IRequestSender<SimpleResponse>>(c => new PostRequestSender<SimpleResponse>(EXTERNAL_MONITOR_API_URL, "activateTransactionMonitor", API_KEY, c.Resolve<ChecksumCalculator>(), c.Resolve<AuthenticationTokenGetter<AuthenticationTokenResponse>>(), API_VERSION, LogManager.GetLogger(typeof(PostRequestSender<SimpleResponse>)))).Named<IRequestSender<SimpleResponse>>("ActivateTransactionMonitor");

            builder.Register<IRequestSender<TransactionMonitorResponse>>(c => new GetRequestSender<TransactionMonitorResponse>(EXTERNAL_MONITOR_API_URL, "transactionTests", API_KEY, c.Resolve<ChecksumCalculator>(), c.Resolve<AuthenticationTokenGetter<AuthenticationTokenResponse>>(), API_VERSION, LogManager.GetLogger(typeof(GetRequestSender<TransactionMonitorResponse>)))).Named<IRequestSender<TransactionMonitorResponse>>("GetTransactionMonitors");
            builder.Register<IRequestSender<TransactionMonitorInfoResponse>>(c => new GetRequestSender<TransactionMonitorInfoResponse>(EXTERNAL_MONITOR_API_URL, "transactionTestInfo", API_KEY, c.Resolve<ChecksumCalculator>(), c.Resolve<AuthenticationTokenGetter<AuthenticationTokenResponse>>(), API_VERSION, LogManager.GetLogger(typeof(GetRequestSender<TransactionMonitorInfoResponse>)))).Named<IRequestSender<TransactionMonitorInfoResponse>>("GetTransactionMonitorInfo");
            builder.Register<IRequestSender<List<TransactionMonitorResultsResponse>>>(c => new GetRequestSender<List<TransactionMonitorResultsResponse>>(EXTERNAL_MONITOR_API_URL, "transactionTestResult", API_KEY, c.Resolve<ChecksumCalculator>(), c.Resolve<AuthenticationTokenGetter<AuthenticationTokenResponse>>(), API_VERSION, LogManager.GetLogger(typeof(GetRequestSender<List<TransactionMonitorResultsResponse>>)))).Named<IRequestSender<List<TransactionMonitorResultsResponse>>>("GetTransactionMonitorResults");
            builder.Register<IRequestSender<List<StepResultsResponse>>>(c => new GetRequestSender<List<StepResultsResponse>>(EXTERNAL_MONITOR_API_URL, "transactionStepResult", API_KEY, c.Resolve<ChecksumCalculator>(), c.Resolve<AuthenticationTokenGetter<AuthenticationTokenResponse>>(), API_VERSION, LogManager.GetLogger(typeof(GetRequestSender<List<StepResultsResponse>>)))).Named<IRequestSender<List<StepResultsResponse>>>("GetTransactionMonitorStepResults");           
            builder.Register<IRequestSender<StepCaptureResponse>>(c => new GetRequestSender<StepCaptureResponse>(EXTERNAL_MONITOR_API_URL, "transactionStepCapture", API_KEY, c.Resolve<ChecksumCalculator>(), c.Resolve<AuthenticationTokenGetter<AuthenticationTokenResponse>>(), API_VERSION, LogManager.GetLogger(typeof(GetRequestSender<StepCaptureResponse>)))).Named<IRequestSender<StepCaptureResponse>>("GetTransactionMonitorStepCapture");
            builder.Register<IRequestSender<StepNetResponse>>(c => new GetRequestSender<StepNetResponse>(EXTERNAL_MONITOR_API_URL, "transactionStepNet", API_KEY, c.Resolve<ChecksumCalculator>(), c.Resolve<AuthenticationTokenGetter<AuthenticationTokenResponse>>(), API_VERSION, LogManager.GetLogger(typeof(GetRequestSender<StepNetResponse>)))).Named<IRequestSender<StepNetResponse>>("GetTransactionMonitorStepNet");              
            // transaction monitor requests

            // transaction monitor classes
            builder.Register(c => new TransactionMonitorActivator(c.Resolve<IRequestSender<SimpleResponse>>("ActivateTransactionMonitor"), LogManager.GetLogger(typeof(TransactionMonitorActivator))));
            builder.Register(c => new TransactionMonitorSuspender(c.Resolve<IRequestSender<TransactionMonitorSuspendResponse>>("SuspendTransactionMonitor"), LogManager.GetLogger(typeof(TransactionMonitorSuspender))));

            builder.Register(c => new StepCaptureGetter(c.Resolve<IRequestSender<StepCaptureResponse>>("GetTransactionMonitorStepCapture"), LogManager.GetLogger(typeof(StepCaptureGetter))));
            builder.Register(c => new StepNetGetter(c.Resolve<IRequestSender<StepNetResponse>>("GetTransactionMonitorStepNet"), LogManager.GetLogger(typeof(StepNetGetter))));
            builder.Register(c => new StepResultsGetter(c.Resolve<IRequestSender<List<StepResultsResponse>>>("GetTransactionMonitorStepResults"), LogManager.GetLogger(typeof(StepResultsGetter))));
            builder.Register(c => new TransactionMonitorGetter(c.Resolve<IRequestSender<TransactionMonitorResponse>>("GetTransactionMonitors"), LogManager.GetLogger(typeof(StepResultsGetter))));
            builder.Register(c => new TransactionMonitorInfoGetter(c.Resolve<IRequestSender<TransactionMonitorInfoResponse>>("GetTransactionMonitorInfo"), LogManager.GetLogger(typeof(StepResultsGetter))));
            builder.Register(c => new TransactionMonitorResultsGetter(c.Resolve<IRequestSender<List<TransactionMonitorResultsResponse>>>("GetTransactionMonitorResults"), LogManager.GetLogger(typeof(StepResultsGetter))));

            // transaction monitor classes
        }
    }
}