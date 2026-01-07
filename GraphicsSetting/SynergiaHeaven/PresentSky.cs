using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Utilities;
using static Synergia.Reassures.Reassures.RTextures;

namespace Synergia.GraphicsSetting.SynergiaHeaven {
    public class PresentSky : CustomSky {
        struct PresentEntity {
            const int MAX_FRAMES = 4;
            const int FRAME_RATE = 6;
            Texture2D _texture;
            public Vector2 Position;
            public float Depth;
            public int FrameHeight;
            public int FrameWidth;
            public float Speed;
            public bool Active;
            int _frame;

            public Texture2D Texture {
                readonly get => _texture;
                set {
                    _texture = value;
                    FrameWidth = value.Width;
                    FrameHeight = value.Height / 4;
                }
            }
            public int Frame {
                readonly get => _frame;
                set {
                    _frame = value % 24;
                }
            }

            public readonly Rectangle GetSourceRectangle() => new(0, _frame / 6 * FrameHeight, FrameWidth, FrameHeight);
        }

        PresentEntity[] _present;
        readonly UnifiedRandom _random = new();
        int _presentRemaining;
        bool _isActive;
        bool _isLeaving;

        public override void OnLoad() {
            GenerateSlimes();
        }
        private void GenerateSlimes() {
            _present = new PresentEntity[Main.maxTilesY / 6];
            for (int i = 0; i < _present.Length; i++) {
                int num = (int)((double)Main.screenPosition.Y * 0.7 - (double)Main.screenHeight);
                int minValue = (int)((double)num - Main.worldSurface * 16.0);
                _present[i].Position = new Vector2(_random.Next(0, Main.maxTilesX) * 16, _random.Next(minValue, num));
                _present[i].Speed = 5f + 3f * (float)_random.NextDouble();
                _present[i].Depth = (float)i / (float)_present.Length * 1.75f + 1.6f;
                _present[i].Texture = Present[_random.Next(2)].Value;
                if (_random.NextBool(60)) {
                    _present[i].Texture = Present[2].Value;
                    _present[i].Speed = 6f + 3f * (float)_random.NextDouble();
                    _present[i].Depth += 0.5f;
                }
                else if (_random.NextBool(30)) {
                    _present[i].Texture = Present[2].Value;
                    _present[i].Speed = 6f + 2f * (float)_random.NextDouble();
                }

                _present[i].Active = true;
            }

            _presentRemaining = _present.Length;
        }
        public override void Update(GameTime gameTime) {
            if (Main.gamePaused || !Main.hasFocus)
                return;

            for (int i = 0; i < _present.Length; i++) {
                if (!_present[i].Active)
                    continue;

                _present[i].Frame++;
                _present[i].Position.Y += _present[i].Speed;
                if (!((double)_present[i].Position.Y > Main.worldSurface * 16.0))
                    continue;

                if (!_isLeaving) {
                    _present[i].Depth = (float)i / (float)_present.Length * 1.75f + 1.6f;
                    _present[i].Position = new Vector2(_random.Next(0, Main.maxTilesX) * 16, -100f);
                    _present[i].Texture = Present[_random.Next(2)].Value;
                    _present[i].Speed = 5f + 3f * (float)_random.NextDouble();
                    if (_random.NextBool(60)) {
                        _present[i].Texture = Present[_random.Next(1)].Value;
                        _present[i].Speed = 6f + 3f * (float)_random.NextDouble();
                        _present[i].Depth += 0.5f;
                    }
                    else if (_random.NextBool(30)) {
                        _present[i].Texture = Present[2].Value;
                        _present[i].Speed = 6f + 2f * (float)_random.NextDouble();
                    }
                }
                else {
                    _present[i].Active = false;
                    _presentRemaining--;
                }
            }

            if (_presentRemaining == 0)
                _isActive = false;
        }
        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth) {
            if (Main.screenPosition.Y > 10000f || Main.gameMenu)
                return;

            int num = -1;
            int num2 = 0;
            for (int i = 0; i < _present.Length; i++) {
                float depth = _present[i].Depth;
                if (num == -1 && depth < maxDepth)
                    num = i;

                if (depth <= minDepth)
                    break;

                num2 = i;
            }

            if (num == -1)
                return;

            Vector2 vector = Main.screenPosition + new Vector2(Main.screenWidth >> 1, Main.screenHeight >> 1);
            Rectangle rectangle = new Rectangle(-1000, -1000, 4000, 4000);
            for (int j = num; j < num2; j++) {
                if (_present[j].Active) {
                    Color color = new Color(Main.ColorOfTheSkies.ToVector4() * 0.9f + new Vector4(0.1f)) * 0.8f;
                    float num3 = 1f;
                    if (_present[j].Depth > 3f)
                        num3 = 0.6f;
                    else if ((double)_present[j].Depth > 2.5)
                        num3 = 0.7f;
                    else if (_present[j].Depth > 2f)
                        num3 = 0.8f;
                    else if ((double)_present[j].Depth > 1.5)
                        num3 = 0.9f;

                    num3 *= 0.8f;
                    color = new Color((int)((float)(int)color.R * num3), (int)((float)(int)color.G * num3), (int)((float)(int)color.B * num3), (int)((float)(int)color.A * num3));
                    Vector2 vector2 = new(1f / _present[j].Depth, 0.9f / _present[j].Depth);
                    Vector2 position = _present[j].Position;
                    position = (position - vector) * vector2 + vector - Main.screenPosition;
                    position.X = (position.X + 500f) % 4000f;
                    if (position.X < 0f)
                        position.X += 4000f;

                    position.X -= 500f;
                    if (rectangle.Contains((int)position.X, (int)position.Y))
                        spriteBatch.Draw(_present[j].Texture, position, _present[j].GetSourceRectangle(), color, 0f, Vector2.Zero, vector2.X * 2f, SpriteEffects.None, 0f);
                }
            }
        }

        public override void Activate(Vector2 position, params object[] args) {
            GenerateSlimes();
            _isActive = true;
            _isLeaving = false;
        }
        public override void Deactivate(params object[] args) {
            _isLeaving = true;
        }
        public override void Reset() {
            _isActive = false;
        }

        public override bool IsActive() => _isActive;
    }
}
