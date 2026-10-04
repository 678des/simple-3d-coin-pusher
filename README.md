# CoinPusherPro - 3Dコインプッシャー開発プロジェクト

## 概要
このプロジェクトは、Unityで構築された物理演算ベースの3Dコインプッシャーゲームです。動的な壁システム、スロットマシンによる報酬メカニクス、フィーバータイム機能を備えています。

## セットアップ手順
1. **エディタ設定**:
   - Unity 2022.3 LTS 以上を使用してください。
   - TextMeshPro パッケージがインポートされていることを確認してください。

2. **Hierarchyの構築**:
   - **GameManager**: 空のGameObjectを作成し、`GameManager.cs` をアタッチ。UI関連のフィールドを紐付けます。
   - **Pusher**: 物理演算で動く台に `Rigidbody` を付け、`PusherMovement.cs` をアタッチします。
   - **Spawners**: `CoinSpawner.cs` をアタッチしたオブジェクトを作成し、必要なPrefabをアサインしてください。

3. **重要事項**:
   - 各マネージャースクリプト（`GameManager`, `PhysicsManager`, `SoundManager`）は、実行時に自動でロードまたは検索されるように設計されていますが、明示的にシーンに配置することで確実な動作を保証します。

## 特徴
- **自動UI構築**: `UIManager` は設定がない場合でもフォールバックとしてUIを動的生成します。
- **手続き的演出**: `EffectsManager` はPrefabなしで実行時にパーティクルを生成します。
- **スロットシステム**: `SlotMachineController` がプッシャー上のエリア判定を自動的に構築し、アーケードゲーム体験を提供します。