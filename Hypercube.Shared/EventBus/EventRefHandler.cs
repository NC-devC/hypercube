﻿using Hypercube.Shared.EventBus.Events;

namespace Hypercube.Shared.EventBus;

public delegate void EventRefHandler<T>(ref T args)
    where T : IEventArgs;