# CoinPusherPro - 開発・セットアップガイド

このプロジェクトは、Unityで構築された物理ベースのコインプッシャーゲームです。

## プロジェクトの開き方
1. Unity Hubを開き「Add project from disk」を選択します。
2. 本プロジェクトのルートフォルダを選択して開きます。
3. `Assets/Scenes/MainGame.unity` を開いて作業を開始してください。

## シーン構成とセットアップ方法
### 1. マネージャーオブジェクト
以下の空のGameObjectを作成し、対応するスクリプトをアタッチしてください：
- **GameManager**: `GameManager.cs`, `UIManager.cs` をアタッチ。
- **PhysicsManagers**: `GlobalPhysicsManager.cs`, `PhysicsManager.cs` をそれぞれ別々にアタッチ。
- **SoundManager**: `SoundManager.cs` をアタッチ。

### 2. 主要ゲームプレイ要素
- **Pusher**: `PusherMovement.cs` をアタッチ。移動速度と範囲をInspectorで設定します。
- **CoinSpawner**: `CoinSpawner.cs` をアタッチ。`coinPrefab` を必ず指定してください。
- **ScoreZone**: 落下口のColliderに `ScoreZone.cs` を追加してください（IsTriggerをONにしてください）。

### 3. 注意点
- **TextMeshPro**: UI表示には TextMeshPro を使用しています。Unityの Package Manager で導入されていることを確認してください。
- **動的生成**: 一部のUIや壁、エフェクトはスクリプト内で自動生成されるため、Hierarchyが空でも動作しますが、必要に応じて調整可能です。