import "../styles.global.css";
import "isomorphic-unfetch";

import { Center, ChakraProvider, Spinner } from "@chakra-ui/react";
import { AuthContext } from "contexts/AuthContext";
import { AuthStage, useAuth } from "hooks/useAuth";
import { AppPropsType } from "next/dist/next-server/lib/utils";
import Head from "next/head";
import { withApplicationInsights } from "next-applicationinsights";
import { I18nProvider } from "next-rosetta";
import React, { ReactElement, useEffect } from "react";
import EnvSettings from "types/EnvSettings";
import isomorphicEnvSettings, { setEnvSettings } from "utils/envSettings";
import { logger } from "utils/logger";
import { openSignInWindow } from "utils/openPopup";

import theme from "../theme/theme";

type Props = {
  envSettings: EnvSettings;
};

const skipauth = "/logincallback";

const MyApp = ({ Component, pageProps, __N_SSG, router }: AppPropsType & Props): ReactElement => {
  // usePWA(); //! OPT IN

  const auth = useAuth(skipauth);

  useEffect(() => {
    if (!__N_SSG) {
      logger.info("Environment should be readable");

      const envSettings = isomorphicEnvSettings();
      if (envSettings) setEnvSettings(envSettings);
      if (process.browser) {
        fetch("/api/getEnv")
          .then(res => {
            if (res.ok) return res.json();
            throw res.statusText;
          })
          .then(
            envSettings => setEnvSettings(envSettings),
            e => {
              logger.debug("env error", e);
            }
          );
      }
    }
  }, []);

  useEffect(() => {
    const envSettings = isomorphicEnvSettings();

    if (auth.authStage == AuthStage.UNAUTHENTICATED && router.pathname != skipauth) {
      openSignInWindow(envSettings.backendUrl + "/api/auth/login", "login", e => {
        if (typeof e.data == "string") {
          console.log(e.source);
          try {
            const pairs = (e.data as string).substring(1).split("&");

            let i: string;
            for (i in pairs) {
              if (pairs[i] === "") continue;

              const pair = pairs[i].split("=");
              const key = decodeURIComponent(pair[0]);
              const value = decodeURIComponent(pair[1]);
              console.log(key, value);

              if (key == "token") auth.login(value);
            }
          } catch (e) {
            console.error(e, e.data);
          }
        }
      });
    }
  }, [auth, auth.authStage, auth.login]);

  return (
    <main>
      <Head>
        <title>IT Minds Coffee Bot</title>
        <meta name="viewport" content="initial-scale=1.0, width=device-width" />
        <meta charSet="utf-8" />
        <meta name="theme-color" content="#2196f3" />
        <meta name="description" content="IT Minds Coffee Bot" />
        <meta name="robots" content="noindex" />

        <link rel="manifest" href="/manifest.json" />
        <link rel="apple-touch-icon" href="/images/icons/icon-192x192.png"></link>
      </Head>
      <noscript>
        <h1>JavaScript must be enabled!</h1>
      </noscript>
      <I18nProvider table={pageProps.table}>
        <ChakraProvider theme={theme}>
          {auth.authStage != AuthStage.AUTHENTICATED && router.pathname != skipauth ? (
            <Center>
              <Spinner
                thickness="4px"
                speed="0.65s"
                emptyColor="gray.200"
                color="blue.500"
                size="xl"
              />
            </Center>
          ) : (
            <>
              <AuthContext.Provider value={auth}>
                {/* <SignalRContext.Provider value={{ connection }}> */}
                <Component {...pageProps} />
                {/* </SignalRContext.Provider> */}
              </AuthContext.Provider>
            </>
          )}
        </ChakraProvider>
      </I18nProvider>
    </main>
  );
};

export default withApplicationInsights({
  instrumentationKey: "07b2bb65-4d30-4c57-b88d-14872abd7676",
  isEnabled: true //process.env.NODE_ENV === 'production'
})(MyApp as any);

// export default MyApp;
