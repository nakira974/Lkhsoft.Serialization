using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using Lkhsoft.Serialization.Activities.Properties;
using Lkhsoft.Utility.Serialization;
using Lkhsoft.Utility.Serialization.Implementations;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;

namespace Lkhsoft.Serialization.Activities
{
    [LocalizedDisplayName(nameof(Resources.JsonSerialization_DisplayName))]
    [LocalizedDescription(nameof(Resources.JsonSerialization_Description))]
    public class JsonSerialization : ContinuableAsyncCodeActivity
    {
        #region Properties

        private IJsonSerializer _jsonSerializer { get; init; }
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
            _jsonSerializer = new JsonSerializer();
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
            
            var @object = JsonObject.Get(context);
            try
            {
                var result = await _jsonSerializer.SerializeAsync<>(@object);
                if (result is null or {Length:0}) throw new NullReferenceException("Serialized Json object is null or empty!");
                try
                {
                    JsonObject.Set(context, result);
                }
                catch ( ArgumentNullException e)
                {
                    throw new NullReferenceException("Result context object is null!", e);
                }
                
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Serialization error!", e);
            }
        }

        #endregion
    }
}

