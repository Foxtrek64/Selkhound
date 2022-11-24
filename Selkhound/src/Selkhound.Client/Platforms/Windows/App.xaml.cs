﻿//
//  App.xaml.cs
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

namespace Selkhound.Client.WinUI
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : MauiWinUIApplication
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// This is the first line of authored code executed, and as such
        /// is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Creates a new Maui App.
        /// </summary>
        /// <returns>A new Maui App.</returns>
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}
