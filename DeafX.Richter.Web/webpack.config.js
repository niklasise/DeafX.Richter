const path = require('path');
const webpack = require('webpack');
const CheckerPlugin = require('awesome-typescript-loader').CheckerPlugin;
const ExtractTextPlugin = require('extract-text-webpack-plugin');
const UglifyJSPlugin = require('uglifyjs-webpack-plugin');

module.exports = (env) => {
    const isDevBuild = !(env && env.prod);
    const clientBundleOutputDir = './wwwroot/dist/js';

    // Configuration in common to both client-side and server-side bundles
    return {
        entry: {
            app: ["whatwg-fetch", "./ClientApp/Index.tsx"],
            css: "./ClientApp/Styles/Main.scss"
        },
        resolve: { extensions: ['.js', '.jsx', '.ts', '.tsx', '.scss', '.css'] },
        output: {
            path: path.join(__dirname, clientBundleOutputDir),
            filename: '[name].js',
            publicPath: 'dist/js/'
        },
        module: {
            rules: [
                { test: /\.tsx?$/, include: /ClientApp/, use: ['babel-loader', 'awesome-typescript-loader?silent=true'] },
                { test: /\.css$/, include: /ClientApp/, use: ExtractTextPlugin.extract(['css-loader?importLoaders=1']) },
                { test: /\.(sass|scss)$/, use: ExtractTextPlugin.extract(['css-loader', 'sass-loader']) }
            ]
        },
        plugins: [
            new CheckerPlugin(),
            new ExtractTextPlugin({ // define where to save the file
                filename: '../css/app.css',
                allChunks: true,
                
            }),
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
                new UglifyJSPlugin()
            ])
    };

};
