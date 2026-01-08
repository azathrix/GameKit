<h1 align="center">Game Kit</h1>

<p align="center">
  Unity 游戏开发工具包，提供常用的扩展方法、对象池、脚本基类等实用工具
</p>

<p align="center">
  <a href="https://github.com/Azathrix/GameKit"><img src="https://img.shields.io/badge/GitHub-GameKit-black.svg" alt="GitHub"></a>
  <a href="https://www.npmjs.com/package/com.azathrix.game-kit"><img src="https://img.shields.io/npm/v/com.azathrix.game-kit.svg" alt="npm"></a>
  <a href="https://github.com/Azathrix/GameKit/blob/main/LICENSE"><img src="https://img.shields.io/badge/license-MIT-blue.svg" alt="License"></a>
  <a href="https://unity.com/"><img src="https://img.shields.io/badge/Unity-6000.3+-black.svg" alt="Unity"></a>
</p>

---

## 特性

- GameScript 增强基类，自动引用赋值
- 高性能 GameObject 对象池
- 丰富的扩展方法（Transform、Vector、Collection 等）
- 常用工具类（随机、数学、时间、单例）

## 安装

### 方式一：Package Manager（推荐）

1. 打开 `Edit > Project Settings > Package Manager`
2. 在 `Scoped Registries` 中添加：
   - Name: `Azathrix`
   - URL: `https://registry.npmjs.org`
   - Scope(s): `com.azathrix`
3. 点击 `Save`
4. 打开 `Window > Package Manager`
5. 切换到 `My Registries`
6. 找到 `Game Kit` 并安装

### 方式二：修改 manifest.json

在 `Packages/manifest.json` 中添加：

```json
{
  "scopedRegistries": [
    {
      "name": "Azathrix",
      "url": "https://registry.npmjs.org",
      "scopes": ["com.azathrix"]
    }
  ],
  "dependencies": {
    "com.azathrix.game-kit": "*"
  }
}
```

### 方式三：Git URL

1. 打开 `Window > Package Manager`
2. 点击 `+` > `Add package from git URL...`
3. 输入：`https://github.com/Azathrix/GameKit.git`

> ⚠️ Git 方式无法自动解析依赖，需要先手动安装：
> - [Azathrix Framework](https://github.com/Azathrix/AzathrixFramework.git)
> - [UniTask](https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask)

## 功能

### GameScript 基类

增强的 MonoBehaviour 基类，提供更清晰的生命周期和自动引用赋值。

```csharp
public class Player : GameScript
{
    [SerializeField, FindRef("Weapon")] private Transform _weapon;      // 按路径查找
    [SerializeField, GetInChildren] private Animator _animator;         // 从子节点获取
    [SerializeField, GetInParent] private Canvas _canvas;               // 从父节点获取
    [SerializeField, Required] private Rigidbody _rb;                   // 必须存在的引用

    protected override void OnScriptInitialize()
    {
        // 替代 Awake
    }

    protected override void OnScriptStart()
    {
        // 替代 Start
    }

    protected override void OnEventRegister()
    {
        // 注册事件监听
    }

    protected override void OnEventUnRegister()
    {
        // 注销事件监听
    }
}
```

### 对象池

高性能 GameObject 对象池，支持预热和异步预热。

```csharp
// 创建池
var bulletPool = new GameObjectPool(bulletPrefab, poolParent, maxSize: 100);

// 预热
bulletPool.Prewarm(20);
await bulletPool.PrewarmAsync(50, countPerFrame: 5);

// 生成
var bullet = bulletPool.Spawn(position, rotation);
var bulletComp = bulletPool.Spawn<Bullet>(position, rotation);

// 回收
bulletPool.Despawn(bullet);

// 实现 IPoolable 接口获取生命周期回调
public class Bullet : MonoBehaviour, IPoolable
{
    public void OnSpawn() { }
    public void OnDespawn() { }
}
```

### 扩展方法

丰富的扩展方法，简化常见操作：

```csharp
// Transform
transform.SetX(10);
transform.SetLocalY(5);
transform.ResetLocal();

// Vector
var v = new Vector3(1, 2, 3);
v.WithX(10);           // (10, 2, 3)
v.Flat();              // (1, 0, 3)

// GameObject
gameObject.SetLayerRecursively(LayerMask.NameToLayer("Enemy"));
gameObject.GetOrAddComponent<Rigidbody>();

// Collection
var list = new List<int> { 1, 2, 3 };
list.Random();         // 随机元素
list.Shuffle();        // 打乱顺序

// Color
color.WithAlpha(0.5f);

// String
"hello_world".ToPascalCase();  // "HelloWorld"
"100".ToInt();                  // 100
"3.14".ToFloat();               // 3.14f

// Animator
animator.SetTriggerSafe("Attack");

// Async
await 0.5f;            // 等待 0.5 秒
await 3;               // 等待 3 帧
```

### 工具类

```csharp
// 随机
RandomUtils.Range(1, 10);
RandomUtils.Bool();
RandomUtils.PointInCircle(radius);
RandomUtils.PointOnCircle(radius);

// 数学
MathUtils.Remap(value, 0, 100, 0, 1);
MathUtils.SmoothDamp(...);

// 时间
TimeUtils.UnixTimestamp;
TimeUtils.FormatDuration(seconds);

// 单例
public class GameManager : Singleton<GameManager> { }
```

## 依赖

- [Azathrix Framework](https://www.npmjs.com/package/com.azathrix.framework)
- [UniTask](https://github.com/Cysharp/UniTask)

## License

MIT
