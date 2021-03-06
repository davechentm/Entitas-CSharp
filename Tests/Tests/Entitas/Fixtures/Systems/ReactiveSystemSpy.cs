using System;
using System.Collections.Generic;
using Entitas;

public interface IReactiveSystemSpy {
    int didInitialize { get; }
    int didExecute { get; }
    int didCleanup { get; }
    int didTearDown { get; }
    Entity[] entities { get; }
}

public class ReactiveSystemSpy : ReactiveSystem, IReactiveSystemSpy, IInitializeSystem, ICleanupSystem, ITearDownSystem {

    public int didInitialize { get { return _didInitialize; } }
    public int didExecute { get { return _didExecute; } }
    public int didCleanup { get { return _didCleanup; } }
    public int didTearDown { get { return _didTearDown; } }
    public Entity[] entities { get { return _entities; } }

    public Action<List<Entity>> executeAction;

    protected int _didInitialize;
    protected int _didExecute;
    protected int _didCleanup;
    protected int _didTearDown;
    protected Entity[] _entities;

    readonly Func<Entity, bool> _filter;

    public ReactiveSystemSpy(EntityCollector collector) : base(collector) {
    }

    public ReactiveSystemSpy(EntityCollector collector, Func<Entity, bool> filter) : base(collector) {
        _filter = filter;
    }

    public void Initialize() {
        _didInitialize += 1;
    }

    protected override bool Filter(Entity entity) {
        return _filter == null || _filter(entity);
    }

    public override void Execute(List<Entity> entities) {
        _didExecute += 1;

        if(entities != null) {
            _entities = entities.ToArray();
        } else {
            _entities = null;
        }

        if(executeAction != null) {
            executeAction(entities);
        }
    }

    public void Cleanup() {
        _didCleanup += 1;
    }

    public void TearDown() {
        _didTearDown += 1;
    }
}
