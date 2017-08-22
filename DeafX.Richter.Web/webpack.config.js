const path = require('path');
const webpack = require('webpack');
const CheckerPlugin = require('awesome-typescript-loader').CheckerPlugin;

module.exports = (env) => {
    const isDevBuild = !(env && env.prod);
    const clientBundleOutputDir = './wwwroot/dist/js';

    // Configuration in common to both client-side and server-side bundles
    return {
        entry: {
            app: "./ClientApp/App.tsx"
        },
        resolve: { extensions: ['.js', '.jsx', '.ts', '.tsx'] },
        output: {
            path: path.join(__dirname, clientBundleOutputDir),
            filename: '[name].js',
            publicPath: 'dist/js/'
        },
        module: {
            rules: [
                { test: /\.tsx?$/, include: /ClientApp/, use: ['babel-loader', 'awesome-typescript-loader?silent=true'] }
            ]
        },
        plugins: [
            new CheckerPlugin(),
            new webpack.DllReferencePlugin({
                context: __dirname,
                manifest: require('./wwwroot/dist/vendor-manifest.json')
            })
        ].concat(isDevBuild ?
            [
                // Plugins that apply in development builds only
                new webpack.SourceMapDevToolPlugin({
                    filename: '[file].map', // Remove this line if you prefer inline source maps
                    moduleFilenameTemplate: path.relative(clientBundleOutputDir, '[resourcePath]') // Point sourcemap entries to the original file locations on disk
                })]
            : [
                // Plugins that apply in production builds only
                new webpack.optimize.UglifyJsPlugin()
            ])
    };

};
