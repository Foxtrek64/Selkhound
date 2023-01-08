//
//  ExampleJsInterop.cs
//
//  Author:
//       LuzFaltex Contributors
//
//  LGPL-3.0 License
//
//  Copyright (c) 2022 LuzFaltex
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using Microsoft.JSInterop;

namespace Selkhound.Client.Shared;

// This class provides an example of how JavaScript functionality can be wrapped
// in a .NET class for easy consumption. The associated JavaScript module is
// loaded on demand when first needed.
//
// This class can be registered as scoped DI service and then injected into Blazor
// components for use.

/// <summary>
/// Wraps a sample js component in C#.
/// </summary>
public class ExampleJsInterop : IAsyncDisposable
{
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExampleJsInterop"/> class.
    /// </summary>
    /// <param name="jsRuntime">An injected Javascript runtime.</param>
    public ExampleJsInterop(IJSRuntime jsRuntime)
    {
        _moduleTask = new
        (
            () => jsRuntime.InvokeAsync<IJSObjectReference>
                           (
                               "import",
                               "./_content/Selkhound.Client.Shared/exampleJsInterop.js"
                           )
                           .AsTask()
        );
    }

    /// <summary>
    /// Wraps a call to the Javascript, in this case, the showPrompt() method.
    /// </summary>
    /// <param name="message">The message to show in the prompt.</param>
    /// <returns>The response from the user.</returns>
    public async ValueTask<string> Prompt(string message)
    {
        var module = await _moduleTask.Value;
        return await module.InvokeAsync<string>("showPrompt", message);
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        if (_moduleTask.IsValueCreated)
        {
            var module = await _moduleTask.Value;
            await module.DisposeAsync();
        }
    }
}
