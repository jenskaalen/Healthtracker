
module.exports = {
    watch: true,
    devtool: 'source-map',
    entry: {
        app: ['./node_modules/zone.js/dist/zone.js',
            './node_modules/reflect-metadata/reflect.js',
            './app/app.ts']
    },
    output: {
        filename: "./bundles/[name].js"
    },
    resolve: {
        extensions: ['', '.ts', '.js']
    },
    module: {
        loaders: [
            { test: /\.tsx?$/, loaders: ['ts-loader', 'angular2-template-loader'] },
            {
                test: /\.(html|css)$/,
                loader: 'raw-loader' 
            }
        ]
    }
}