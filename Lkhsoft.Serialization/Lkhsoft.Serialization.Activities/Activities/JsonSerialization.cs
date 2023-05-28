using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using Lkhsoft.Serialization.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;

namespace Lkhsoft.Serialization.Activities
{
    [LocalizedDisplayName(nameof(Resources.JsonSerialization_DisplayName))]
    [LocalizedDescription(nameof(Resources.JsonSerialization_Description))]
    public class JsonSerialization : ContinuableAsyncCodeActivity
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

        [LocalizedDisplayName(nameof(Resources.JsonSerialization_JsonObject_DisplayName))]
        [LocalizedDescription(nameof(Resources.JsonSerialization_JsonObject_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<object> JsonObject { get; set; }

        [LocalizedDisplayName(nameof(Resources.JsonSerialization_Json_DisplayName))]
        [LocalizedDescription(nameof(Resources.JsonSerialization_Json_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public OutArgument<string> Json { get; set; }

        #endregion


        #region Constructors

        public JsonSerialization()
        {
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (JsonObject == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(JsonObject)));

            base.CacheMetadata(metadata);
        }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {
            // Inputs
            var timeout = TimeoutMS.Get(context);
            var jsonobject = JsonObject.Get(context);

            // Set a timeout on the execution
            var task = ExecuteWithTimeout(context, cancellationToken);
            if (await Task.WhenAny(task, Task.Delay(timeout, cancellationToken)) != task) throw new TimeoutException(Resources.Timeout_Error);

            // Outputs
            return (ctx) => {
                Json.Set(ctx, null);
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

