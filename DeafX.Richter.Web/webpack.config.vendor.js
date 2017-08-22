const path = require('path');
const webpack = require('webpack');

module.exports = (env) => {
    const isDevBuild = !(env && env.prod);

    return {
        stats: { modules: false },
        resolve: { extensions: ['.js'] },
        entry: {
            vendor: [
                'react',
                'react-dom',
            ],
        },
        output: {
            path: path.join(__dirname, 'wwwroot', 'dist/js'),
            filename: '[name].js',
            library: '[name]_[hash]',
            publicPath: '/dist/js/'
        },
        plugins: [
            new webpack.DefinePlugin({
                'process.env.NODE_ENV': isDevBuild ? '"development"' : '"production"'
            }),
            new webpack.DllPlugin({
                path: path.join(__dirname, 'wwwroot', 'dist', '[name]-manifest.json'),
                name: '[name]_[hash]'
            })
        ].concat(isDevBuild ? [] : [
            new webpack.optimize.UglifyJsPlugin()
        ])
    };
};
