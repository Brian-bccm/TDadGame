using System.Collections.Generic;
using KingdomLastStand.Battle;
using KingdomLastStand.Combat;
using KingdomLastStand.Data;
using KingdomLastStand.Economy;
using KingdomLastStand.Enemies;
using KingdomLastStand.Units;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KingdomLastStand.Demo
{
    public sealed class DemoBattleBootstrap : MonoBehaviour
    {
        private DemoBoard _board;
        private BattleController _battle;
        private CastleController _castle;
        private WaveManager _waves;
        private bool _built;
        private GUIStyle _headerStyle;
        private GUIStyle _bodyStyle;

        private void Awake()
        {
            if (_built) return;
            _built = true;
            Application.targetFrameRate = 60;
            Screen.orientation = ScreenOrientation.Portrait;
            BuildArena();
        }

        private void BuildArena()
        {
            BuildCameraAndLight();
            var world = new GameObject("Demo World");
            CreateBlock(world.transform, "Meadow", Vector3.zero, new Vector3(13f, 20f, 0.8f), new Color(0.21f, 0.39f, 0.27f), 4f);
            CreateBlock(world.transform, "Road", new Vector3(0f, 0f, 2f), new Vector3(2.3f, 18f, 0.4f), new Color(0.72f, 0.61f, 0.42f), 3f);
            CreateBlock(world.transform, "Road Edge Left", new Vector3(-1.3f, 0f, 2.2f), new Vector3(0.12f, 18f, 0.4f), new Color(0.39f, 0.3f, 0.21f), 3f);
            CreateBlock(world.transform, "Road Edge Right", new Vector3(1.3f, 0f, 2.2f), new Vector3(0.12f, 18f, 0.4f), new Color(0.39f, 0.3f, 0.21f), 3f);

            var path = new[]
            {
                NewWaypoint(world.transform, "Path 0", new Vector3(0f, 8.5f, 0f)),
                NewWaypoint(world.transform, "Path 1", new Vector3(0f, 4.5f, 0f)),
                NewWaypoint(world.transform, "Path 2", new Vector3(0f, 0.5f, 0f)),
                NewWaypoint(world.transform, "Path 3", new Vector3(0f, -3.5f, 0f)),
                NewWaypoint(world.transform, "Path 4", new Vector3(0f, -7.2f, 0f))
            };

            var castleObject = CreateBlock(world.transform, "Castle", new Vector3(0f, -8.1f, 0f), new Vector3(2.6f, 1.1f, 0.8f), new Color(0.22f, 0.46f, 0.84f), 0.6f);
            var castleHealth = castleObject.AddComponent<Damageable>();
            castleHealth.Configure(20f);
            _castle = castleObject.AddComponent<CastleController>();

            var wallet = new CurrencyWallet(gold: 100);
            var templates = new GameObject("Runtime Templates");
            templates.SetActive(false);
            var projectilePrefab = CreateProjectileTemplate(templates.transform);
            var archerPrefab1 = CreateUnitTemplate(templates.transform, "Archer Tier 1", new Color(0.25f, 0.72f, 0.92f), 0.82f);
            var archerPrefab2 = CreateUnitTemplate(templates.transform, "Ranger Tier 2", new Color(0.34f, 0.9f, 0.66f), 1.02f);
            var archerPrefab3 = CreateUnitTemplate(templates.transform, "Royal Archer Tier 3", new Color(1f, 0.75f, 0.23f), 1.22f);
            var magePrefab = CreateUnitTemplate(templates.transform, "Mage", new Color(0.76f, 0.37f, 0.9f), 0.92f);
            var archer3 = CreateUnitData("archer.t3", "archer", 3, archerPrefab3, projectilePrefab, 17f, 0.55f, 5.5f, 12f, 0, null);
            var archer2 = CreateUnitData("archer.t2", "archer", 2, archerPrefab2, projectilePrefab, 10f, 0.68f, 5f, 12f, 0, archer3);
            var archer1 = CreateUnitData("archer.t1", "archer", 1, archerPrefab1, projectilePrefab, 5f, 0.8f, 4.7f, 12f, 30, archer2);
            var mage = CreateUnitData("mage.t1", "mage", 1, magePrefab, projectilePrefab, 12f, 1.3f, 4.2f, 8f, 50, null);

            var slotPositions = new[]
            {
                new Vector3(-3.6f, -1.8f, -0.4f), new Vector3(-1.2f, -1.8f, -0.4f),
                new Vector3(1.2f, -1.8f, -0.4f), new Vector3(3.6f, -1.8f, -0.4f),
                new Vector3(-3.6f, -4.1f, -0.4f), new Vector3(-1.2f, -4.1f, -0.4f),
                new Vector3(1.2f, -4.1f, -0.4f), new Vector3(3.6f, -4.1f, -0.4f)
            };
            CreateFormationMarkers(world.transform, slotPositions);

            var boardObject = new GameObject("Unit Board");
            _board = boardObject.AddComponent<DemoBoard>();
            _board.Configure(wallet, archer1, mage, slotPositions);
            var enemyPrefab = CreateEnemyTemplate(templates.transform);
            var enemyData = ScriptableObject.CreateInstance<EnemyData>();
            enemyData.Configure("footman", "Footman", enemyPrefab, 28f, 1.05f, 1, 6);
            var wave = ScriptableObject.CreateInstance<WaveData>();
            wave.Configure(new List<WaveData.SpawnGroup>
            {
                new WaveData.SpawnGroup(enemyData, enemyCount: 16, spawnInterval: 0.8f, groupDelay: 0f)
            });

            var waveObject = new GameObject("Wave Manager");
            _waves = waveObject.AddComponent<WaveManager>();
            _waves.Configure(wave, path, _castle);
            _waves.EnemyKilled += enemy => wallet.AddGold(enemy.GoldReward);

            var battleObject = new GameObject("Battle Controller");
            _battle = battleObject.AddComponent<BattleController>();
            _battle.ConfigureForDemo(_waves, _castle);
        }

        private static void BuildCameraAndLight()
        {
            var cameraObject = new GameObject("Main Camera");
            cameraObject.tag = "MainCamera";
            cameraObject.transform.position = new Vector3(0f, 0f, -20f);
            var camera = cameraObject.AddComponent<Camera>();
            camera.orthographic = true;
            camera.orthographicSize = 10f;
            camera.backgroundColor = new Color(0.08f, 0.12f, 0.12f);
            camera.clearFlags = CameraClearFlags.SolidColor;

            var lightObject = new GameObject("Demo Light");
            lightObject.transform.rotation = Quaternion.Euler(0f, 0f, -35f);
            lightObject.AddComponent<Light>().type = LightType.Directional;
        }

        private static GameObject CreateUnitTemplate(Transform parent, string name, Color color, float size)
        {
            var unit = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            unit.name = name;
            unit.transform.SetParent(parent, false);
            unit.transform.localScale = Vector3.one * size;
            Paint(unit, color);
            unit.AddComponent<UnitTower>();
            return unit;
        }

        private static GameObject CreateEnemyTemplate(Transform parent)
        {
            var enemy = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            enemy.name = "Footman Template";
            enemy.transform.SetParent(parent, false);
            enemy.transform.localScale = Vector3.one * 0.72f;
            Paint(enemy, new Color(0.88f, 0.25f, 0.22f));
            enemy.AddComponent<Damageable>();
            enemy.AddComponent<EnemyAgent>();
            return enemy;
        }

        private static GameObject CreateProjectileTemplate(Transform parent)
        {
            var projectile = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            projectile.name = "Arrow Template";
            projectile.transform.SetParent(parent, false);
            projectile.transform.localScale = Vector3.one * 0.22f;
            Paint(projectile, new Color(1f, 0.9f, 0.45f));
            projectile.AddComponent<Projectile>();
            return projectile;
        }

        private static UnitData CreateUnitData(string id, string family, int tier, GameObject prefab,
            GameObject projectile, float damage, float interval, float range, float speed, int cost, UnitData mergeResult)
        {
            var data = ScriptableObject.CreateInstance<UnitData>();
            data.Configure(id, family, tier, prefab, projectile, damage, interval, range, speed, cost, mergeResult);
            data.name = id;
            return data;
        }

        private static Transform NewWaypoint(Transform parent, string name, Vector3 position)
        {
            var point = new GameObject(name);
            point.transform.SetParent(parent, false);
            point.transform.position = position;
            return point.transform;
        }

        private static GameObject CreateBlock(Transform parent, string name, Vector3 position, Vector3 scale, Color color, float z)
        {
            var block = GameObject.CreatePrimitive(PrimitiveType.Cube);
            block.name = name;
            block.transform.SetParent(parent, false);
            block.transform.position = new Vector3(position.x, position.y, z);
            block.transform.localScale = scale;
            Paint(block, color);
            return block;
        }

        private static void CreateFormationMarkers(Transform parent, Vector3[] slots)
        {
            foreach (var slot in slots)
            {
                var marker = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                marker.name = "Unit Slot";
                marker.transform.SetParent(parent, false);
                marker.transform.position = new Vector3(slot.x, slot.y, 1.4f);
                marker.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                marker.transform.localScale = new Vector3(0.9f, 0.035f, 0.9f);
                Paint(marker, new Color(0.83f, 0.82f, 0.62f, 0.8f));
                var collider = marker.GetComponent<Collider>();
                if (collider != null) Destroy(collider);
            }
        }

        private static void Paint(GameObject target, Color color)
        {
            var renderer = target.GetComponent<Renderer>();
            if (renderer == null) return;
            var shader = Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard");
            if (shader == null) return;
            var material = new Material(shader) { color = color };
            renderer.sharedMaterial = material;
        }

        private void OnGUI()
        {
            _headerStyle ??= new GUIStyle(GUI.skin.label) { fontSize = 22, fontStyle = FontStyle.Bold, normal = { textColor = Color.white } };
            _bodyStyle ??= new GUIStyle(GUI.skin.label) { fontSize = 15, wordWrap = true, normal = { textColor = Color.white } };

            const int margin = 12;
            var panelHeight = 184;
            var panel = new Rect(margin, Screen.height - panelHeight - margin, Screen.width - margin * 2, panelHeight);
            GUI.Box(panel, GUIContent.none);
            GUILayout.BeginArea(new Rect(panel.x + 12, panel.y + 8, panel.width - 24, panel.height - 16));
            GUILayout.Label($"KINGDOM LAST STAND     Gold: {_board.Gold}     Castle: {Mathf.CeilToInt(_castle.Health.CurrentHealth)}/{Mathf.CeilToInt(_castle.Health.MaximumHealth)}", _headerStyle);
            GUILayout.Label(_board.Message, _bodyStyle, GUILayout.Height(40));
            GUILayout.BeginHorizontal();
            GUI.enabled = _battle.State == BattleState.Ready || _battle.State == BattleState.Running;
            if (GUILayout.Button("Recruit Archer · 30", GUILayout.Height(44))) _board.BuyArcher();
            if (GUILayout.Button("Recruit Mage · 50", GUILayout.Height(44))) _board.BuyMage();
            GUI.enabled = true;
            if (_battle.State == BattleState.Ready)
            {
                if (GUILayout.Button("Start Battle", GUILayout.Height(44))) _battle.StartBattle();
            }
            else if (_battle.State == BattleState.Victory || _battle.State == BattleState.Defeat)
            {
                if (GUILayout.Button("Restart", GUILayout.Height(44)))
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else
            {
                GUILayout.Label("WAVE IN PROGRESS", GUILayout.Height(44));
            }
            GUILayout.EndHorizontal();
            if (_battle.State == BattleState.Victory)
                GUILayout.Label("VICTORY! The road is safe. Recruit and merge units, then replay.", _bodyStyle);
            else if (_battle.State == BattleState.Defeat)
                GUILayout.Label("The castle fell. Add more defenders and try again.", _bodyStyle);
            GUILayout.EndArea();
        }
    }
}
