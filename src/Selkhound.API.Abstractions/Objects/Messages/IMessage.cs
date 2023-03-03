using Remora.Rest.Core;

namespace Selkhound.API.Abstractions.Objects.Messages
{
    /// <summary>
    /// Represents a message sent by a user, webhook, or bot.
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// Gets the unique id of this message.
        /// </summary>
        Snowflake Id { get; }

        /// <summary>
        /// Gets the creator of this message.
        /// </summary>
        OneOf<IUser, IWebhook> Author { get; }
    }
}
