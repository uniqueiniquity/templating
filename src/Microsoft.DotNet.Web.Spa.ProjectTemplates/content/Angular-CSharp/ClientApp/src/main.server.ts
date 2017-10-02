import 'reflect-metadata';
import 'zone.js';
import 'rxjs/add/operator/first';
import { APP_BASE_HREF } from '@angular/common';
import { enableProdMode, ApplicationRef, NgModule, NgZone, ValueProvider } from '@angular/core';
import { platformDynamicServer, PlatformState, INITIAL_CONFIG } from '@angular/platform-server';
import { AppServerModule } from './app/app.server.module';
import { createServerRenderer, RenderResult } from 'aspnet-prerendering';
import * as requestPromise from 'request-promise-native';

enableProdMode();

export default createServerRenderer(params => {
    // Unfortunately @angular/cli's builds for 'server' platform don't emit the index.html
    // template, and we can't even use the original index.html file on disk because that
    // doesn't have the scripts/CSS injected into it. So for now, make an HTTP request back
    // to the host app to get the build index.html from either CLI middleware (in dev) or
    // the built file on disk (in prod). Longer term, would be better to work with the
    // @angular/cli project to add an option to emit index.html on 'server' builds.
    // TODO: Don't re-fetch this on every request. It won't change between app restarts.
    const templateRequest: Promise<string> = requestPromise(params.data.templateUrl);

    return templateRequest.then(templateString => {
        const providers = [
            { provide: INITIAL_CONFIG, useValue: { document: templateString, url: params.url } },
            { provide: APP_BASE_HREF, useValue: params.baseUrl },
            { provide: 'BASE_URL', useValue: params.origin + params.baseUrl },
        ];

        return platformDynamicServer(providers).bootstrapModule(AppServerModule).then(moduleRef => {
            const appRef: ApplicationRef = moduleRef.injector.get(ApplicationRef);
            const state = moduleRef.injector.get(PlatformState);
            const zone = moduleRef.injector.get(NgZone);

            return new Promise<RenderResult>((resolve, reject) => {
                zone.onError.subscribe((errorInfo: any) => reject(errorInfo));
                appRef.isStable.first(isStable => isStable).subscribe(() => {
                    // Because 'onStable' fires before 'onError', we have to delay slightly before
                    // completing the request in case there's an error to report
                    setImmediate(() => {
                        resolve({
                            html: state.renderToString()
                        });
                        moduleRef.destroy();
                    });
                });
            });
        });
    });
});
