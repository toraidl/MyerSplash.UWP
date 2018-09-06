using System;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.AI.MachineLearning;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Media;
using System.Collections.Generic;
using System.Linq;

namespace MyerSplashShared.MachineLeanrning
{
    internal sealed class Input
    {
        public ImageFeatureValue data; // shape(1,3,224,224)
    }

    internal sealed class Output
    {
        public TensorFloat data; // shape(1,1000,1,1)
    }

    internal sealed class MlModel
    {
        private LearningModel model;
        private LearningModelSession session;
        private LearningModelBinding binding;

        public static async Task<MlModel> CreateFromStreamAsync(IRandomAccessStreamReference stream)
        {
            MlModel learningModel = new MlModel
            {
                model = await LearningModel.LoadFromStreamAsync(stream)
            };
            learningModel.session = new LearningModelSession(learningModel.model);
            learningModel.binding = new LearningModelBinding(learningModel.session);
            return learningModel;
        }

        public async Task<Output> EvaluateAsync(Input input)
        {
            binding.Bind("data_0", input.data);
            var result = await session.EvaluateAsync(binding, "0");
            var output = new Output
            {
                data = result.Outputs["softmaxout_1"] as TensorFloat
            };
            return output;
        }
    }

    public class MlModelWrapper
    {
        private MlModel _modelGen = new MlModel();
        private Input _input = new Input();
        private Output _output = new Output();
        private SoftwareBitmap softwareBitmap;

        private async Task LoadModelAsync()
        {
            //Load a machine learning model
            var modelFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri($"ms-appx:///Assets/Model/model.onnx"));
            _modelGen = await MlModel.CreateFromStreamAsync(modelFile as IRandomAccessStreamReference);
        }

        public async Task<string> BeginDetectAsync(StorageFile file)
        {
            await LoadModelAsync();
            if (file != null)
            {
                using (var stream = await file.OpenAsync(FileAccessMode.Read))
                {
                    // Create the decoder from the stream
                    BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);

                    // Get the SoftwareBitmap representation of the file
                    softwareBitmap = await decoder.GetSoftwareBitmapAsync();
                    softwareBitmap = SoftwareBitmap.Convert(softwareBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
                }

                var inputData = ImageFeatureValue.CreateFromVideoFrame(VideoFrame.CreateWithSoftwareBitmap(softwareBitmap));

                _input.data = inputData;
                //Evaluate the model
                _output = await _modelGen.EvaluateAsync(_input);

                //Convert output to datatype
                IReadOnlyList<float> VectorImage = _output.data.GetAsVectorView();
                IList<float> ImageList = VectorImage.ToList();

                softwareBitmap?.Dispose();

                //LINQ query to check for highest probability digit
                var index = ImageList.IndexOf(ImageList.Max());
                return DetectionResults.FindLabel(index).ToUpper();
            }
            else
            {
                return DetectionResults.FindLabel(-1);
            }
        }
    }
}
