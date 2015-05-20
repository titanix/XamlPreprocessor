You need to modify the .csproj on time in the following way:
- import the PreprocessorTask

You need to modify the .csproj for each XAML file (sample.xaml) you want to preprocess in the following way:
- a PreprocessorTask element taking sample.pre.xaml as input and outputing sample.xaml