using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Diagnostics;

using Zmp.Imuz.Communication;
using Zmp.Imuz.Estimator;
using Zmp.Imuz.Kinematics;
using Zmp.Imuz.Draw;

namespace testGUI {
    public partial class Form1 : Form {

        int numberOfLink = 6;
        ICommunicationPort _port;
        CompositePoseEstimator[] _estimator;
        KinematicsViewModel _kvm;

        double scale = 100;

        Link _linkPelvis;
        Link _linkLeftThigh;
        Link _linkLeftShin;
        Link _linkLeftFoot;
        Link _linkRightThigh;
        Link _linkRightShin;
        Link _linkRightFoot;
        List<Link> _allLinkes = new List<Link>();
        List<Shape> _allShapes = new List<Shape>();

        Quaternion quaternionZero = Quaternion.CreateFromAngle(0, 0, 0);

        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            _estimator = new CompositePoseEstimator[numberOfLink];

            // フィルタ：速い動きに追従する設定です
            var fs = 1 / 0.01;
            var cutoff = 25.0;
            var tc = 0.4;
            for (int i = 0; i < numberOfLink; i++) {
                _estimator[i] = new CompositePoseEstimator();
                _estimator[i].SetFilterCoeffs(CompositePoseEstimator.FilterName.AccLowPassFilter, FilterDisignerBiquad.LowPass(fs, cutoff, 1.0));
                _estimator[i].SetFilterCoeffs(CompositePoseEstimator.FilterName.CompLowPassFilter, FilterDisignerBiquad.LowPass(fs, cutoff, 1.0));
                _estimator[i].SetFilterCoeffs(CompositePoseEstimator.FilterName.GyroLowPassFilter, FilterDisignerBiquad.LowPass(fs, 45.0, 1.0));
                _estimator[i].TimeConstantComposition = tc;
            }

            // 接続処理
            var dialog = new PortOpenDialog();
            if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) { return; }

            _port = CommunicationManager.CreateCommunicationPort(dialog.ConnectionNameType);
            // データ更新イベントの登録
            _port.MsgMeasurementReceived += new ReceivedMsgMeasurementHandler(_port_MsgMeasurementReceived);

            dialog.Dispose();
            _port.Open();


            #region 形状、位置設定
            // 骨盤
            _linkPelvis = new Link() {
                LinkId = 0,
                ParentLinkId = -1,
                Position = new Vec4(),
                Orientation = Quaternion.CreateFromAngle(0, 0, 0),
            };
            var _linkPelvisShape = new Shape() {
                Type = ShapeType.Pyramid,
                Scale = new Vec4(30.7 / scale, 3 / scale, 10 / scale, 0 / scale),
                Orientation = Quaternion.CreateFromAngle(0, -Math.PI / 2, 0),
                Position = new Vec4(0, 0, 0, 0),
            };
            _allLinkes.Add(_linkPelvis);
            _allShapes.Add(_linkPelvisShape);

            // 左大腿
            _linkLeftThigh = new Link() {
                LinkId = 1,
                ParentLinkId = 0,
                Position = new Vec4(15.4 / scale, 0 / scale, 0 / scale),
                Orientation = Quaternion.CreateFromAngle(0, 0, 0),
            };
            var _linkLeftThighShape = new Shape() {
                Type = ShapeType.Pyramid,
                Scale = new Vec4(-38.1 / scale, 5 / scale, 3 / scale, 0 / scale),
                Orientation = Quaternion.CreateFromAngle(0, -Math.PI / 2, 0),
                Position = new Vec4(0, 0, 0, 0),
            };
            _allLinkes.Add(_linkLeftThigh);
            _allShapes.Add(_linkLeftThighShape);

            // 左脛
            _linkLeftShin = new Link() {
                LinkId = 2,
                ParentLinkId = 1,
                Position = new Vec4(0 / scale, 0 / scale, -38.1 / scale),
                Orientation = Quaternion.CreateFromAngle(0, 0, 0),
            };
            var _linkLeftShinShape = new Shape() {
                Type = ShapeType.Pyramid,
                Scale = new Vec4(-46.5 / scale, 5 / scale, 3 / scale, 0 / scale),
                Orientation = Quaternion.CreateFromAngle(0, -Math.PI / 2, 0),
                Position = new Vec4(0, 0, 0, 0),
            };
            _allLinkes.Add(_linkLeftShin);
            _allShapes.Add(_linkLeftShinShape);

            // 左足首
            _linkLeftFoot = new Link() {
                LinkId = 3,
                ParentLinkId = 2,
                Position = new Vec4(0 / scale, 0 / scale, -46.5 / scale),
                Orientation = Quaternion.CreateFromAngle(0, 0, 0),
            };
            var _linkLeftFootShape = new Shape() {
                Type = ShapeType.Pyramid,
                Scale = new Vec4(16.5 / scale, 2 / scale, 2 / scale, 0 / scale),
                Orientation = Quaternion.CreateFromAngle(0, Math.PI / 2, -Math.PI / 2),
                Position = new Vec4(0, 0, 0, 0),
            };
            _allLinkes.Add(_linkLeftFoot);
            _allShapes.Add(_linkLeftFootShape);


            // 右大腿
            _linkRightThigh = new Link() {
                LinkId = 4,
                ParentLinkId = 0,
                Position = new Vec4(-15.4 / scale, 0 / scale, 0 / scale),
                Orientation = Quaternion.CreateFromAngle(0, 0, 0),
            };
            var _linkRightThighShape = new Shape() {
                Type = ShapeType.Pyramid,
                Scale = new Vec4(-38.1 / scale, 5 / scale, 3 / scale, 0 / scale),
                Orientation = Quaternion.CreateFromAngle(0, -Math.PI / 2, 0),
                Position = new Vec4(0, 0, 0, 0),
            };
            _allLinkes.Add(_linkRightThigh);
            _allShapes.Add(_linkRightThighShape);

            // 右脛
            _linkRightShin = new Link() {
                LinkId = 5,
                ParentLinkId = 4,
                Position = new Vec4(0 / scale, 0 / scale, -38.1 / scale),
                Orientation = Quaternion.CreateFromAngle(0, 0, 0),
            };
            var _linkRightShinShape = new Shape() {
                Type = ShapeType.Pyramid,
                Scale = new Vec4(-46.5 / scale, 5 / scale, 3 / scale, 0 / scale),
                Orientation = Quaternion.CreateFromAngle(0, -Math.PI / 2, 0),
                Position = new Vec4(0, 0, 0, 0),
            };
            _allLinkes.Add(_linkRightShin);
            _allShapes.Add(_linkRightShinShape);

            // 右足首
            _linkRightFoot = new Link() {
                LinkId = 6,
                ParentLinkId = 5,
                Position = new Vec4(0 / scale, 0 / scale, -46.5 / scale),
                Orientation = Quaternion.CreateFromAngle(0, 0, 0),
            };
            var _linkRightFootShape = new Shape() {
                Type = ShapeType.Pyramid,
                Scale = new Vec4(16.5 / scale, 2 / scale, 2 / scale, 0 / scale),
                Orientation = Quaternion.CreateFromAngle(0, Math.PI / 2, -Math.PI / 2),
                Position = new Vec4(0, 0, 0, 0),
            };
            _allLinkes.Add(_linkRightFoot);
            _allShapes.Add(_linkRightFootShape);

            #endregion

            // キネマティクスの初期化
            _kvm = new KinematicsViewModel(new ForwardKinematicsSolver());
            _kvm.Init(_allLinkes, _allShapes);
            
            // 描画領域に追加
            foreach (var s in _allShapes) { imuzDraw1.AddPyramidModel((float)s.Scale.x, (float)s.Scale.y, (float)s.Scale.z); }

            return;
        }

        void _port_MsgMeasurementReceived(object sender, MeasurementData mdata) {

            var data = mdata.Clone();
            int id = 0, linkID = 0;

            // ID と　姿勢の対応
            switch (mdata.node_no) {
                default: { return; }

                // 左腿
                case 2: { id = 0; linkID = 1; } break;
                // 左脛
                case 3: { id = 1; linkID = 2; } break;
                // 左足首
                case 4: { id = 2; linkID = 3; } break;

                // 右腿
                case 5: { id = 3; linkID = 4; } break;
                // 右脛
                case 6: { id = 4; linkID = 5; } break;
                // 右足首
                case 7: { id = 5; linkID = 6; } break;
            }
            _estimator[id].SetMeasurement(data.acc, data.gyro, data.comp, data.time);

            if (false) {
                _kvm.SetOrientation(linkID, _estimator[id].Pose);
            } else {
                _kvm.SetOrientation(linkID, quaternionZero);
            }
            _kvm.Solve();

            ModelDrawHelper.DrawModel(imuzDraw1, _kvm);

            return;
        }

    }


    static class ModelDrawHelper {
        public static void SetupModel(ImuzDraw draw, KinematicsModel model) {
            foreach (Link l in model.Model) {
                draw.AddPyramidModel((float)0.5, (float)0.1, (float)0.1);
            }
        }

        public static void SetupModel(ImuzDraw draw, KinematicsViewModel model) {
            foreach (Shape s in model.Shape) {
                switch (s.Type) {
                    case ShapeType.Pyramid:
                        draw.AddPyramidModel((float)s.Scale.x, (float)s.Scale.y, (float)s.Scale.z);
                        break;
                    default:
                        continue;
                }
            }
        }

        public static void DrawModel(ImuzDraw draw, KinematicsModel model) {
            foreach (Pose p in model.Result) {
                Mat4 m = p.Matrix;
                draw.GetModel(p.LinkId).SetWorldMatrix(MatrixHelper.ToXna(m));
            }
        }


    }

}
