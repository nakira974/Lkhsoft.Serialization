using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using Lkhsoft.Serialization.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;

namespace Lkhsoft.Serialization.Activities
{
    [LocalizedDisplayName(nameof(Resources.JsonDeserialization_DisplayName))]
    [LocalizedDescription(nameof(Resources.JsonDeserialization_Description))]
    public class JsonDeserialization : ContinuableAsyncCodeActivity
    {
        #region Properties

        /// <summary>
        /// If set, continue executing the remaining activities even if the current activity has failed.
        /// </summary>
        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.ContinueOnError_DisplayName))]
        [LocalizedDescription(nameof(Resources.ContinueOnError_Description))]
        public override InArgument<bool> ContinueOnError { get; set; }

        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.Timeout_DisplayName))]
        [LocalizedDescription(nameof(Resources.Timeout_Description))]
        public InArgument<int> TimeoutMS { get; set; } = 60000;

        [LocalizedDisplayName(nameof(Resources.JsonDeserialization_Json_DisplayName))]
        [LocalizedDescription(nameof(Resources.JsonDeserialization_Json_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> Json { get; set; }

        [LocalizedDisplayName(nameof(Resources.JsonDeserialization_JsonObject_DisplayName))]
        [LocalizedDescription(nameof(Resources.JsonDeserialization_JsonObject_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<object> JsonObject { get; set; }

        #endregion


        #region Constructors

        public JsonDeserialization()
        {
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (Json == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(Json)));

            base.CacheMetadata(metadata);
        }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {
            // Inputs
            var timeout = TimeoutMS.Get(context);
            var json = Json.Get(context);

            // Set a timeout on the execution
            var task = ExecuteWithTimeout(context, cancellationToken);
            if (await Task.WhenAny(task, Task.Delay(timeout, cancellationToken)) != task) throw new TimeoutException(Resources.Timeout_Error);

            // Outputs
            return (ctx) => {
                JsonObject.Set(ctx, null);
            };
        }

        private async Task ExecuteWithTimeout(AsyncCodeActivityContext context, CancellationToken cancellationToken = default)
        {
            ///////////////////////////
            // Add execution logic HERE
            ///////////////////////////
        }

        #endregion
    }
}

