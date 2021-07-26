/* istanbul ignore file */
import { GetServerSidePropsContext } from "next";
import { useRouter } from "next/router";
import { useCallback, useState } from "react";
import { api } from "services/backend/api";
import { AuthClient, UserDTO } from "services/backend/nswagts";

import { useEffectAsync } from "./useEffectAsync";

export enum AuthStage {
  CHECKING,
  AUTHENTICATED,
  UNAUTHENTICATED
}

type AuthHook<T> = {
  authStage: AuthStage;
  login: (s: string) => Promise<boolean>;
  logout: () => void;
  activeUser: T | null;
  checkAuth: () => Promise<void>;
};

export const useAuth = (authSkip: string): AuthHook<UserDTO> => {
  const [authStage, setAuthStage] = useState(AuthStage.CHECKING);
  const [authCounter, setAuthCounter] = useState(0);
  const [activeUser, setActiveUser] = useState<UserDTO>(null);
  const router = useRouter();

  const checkAuth = useCallback(async () => {
    if (router.pathname == authSkip) return;
    setAuthStage(AuthStage.CHECKING);

    const client: AuthClient = await api(AuthClient);
    const user: UserDTO = await client.checkAuth().catch(() => null);

    setActiveUser(user);
    setAuthStage(user ? AuthStage.AUTHENTICATED : AuthStage.UNAUTHENTICATED);
  }, []);

  useEffectAsync(checkAuth, [authCounter]);

  const login = useCallback(async (token: string) => {
    // setCookie(token);
    setAuthToken(token);
    setAuthCounter(c => c + 1);
    return true;
  }, []);

  const logout = useCallback(() => {
    // deleteCookie();
    setAuthToken("");
    setAuthCounter(c => c + 1);
    // router.push("/");
  }, []);

  return { authStage, login, logout, activeUser, checkAuth };
};

export const getAuthToken = (context?: GetServerSidePropsContext): string => {
  if (process.browser) return localStorage.getItem(process.env.NEXT_PUBLIC_AUTH_NAME);

  if (!context) return null;

  // const token = context.req.cookies[process.env.NEXT_PUBLIC_AUTH_NAME];
  return null;
};

export const setAuthToken = (token: string, context?: GetServerSidePropsContext): void => {
  if (process.browser) return localStorage.setItem(process.env.NEXT_PUBLIC_AUTH_NAME, token);

  if (!context) return;

  // context.res.setHeader("Set-Cookie", genSetCookie(token));

  return;
};

// const genSetCookie = (token: string) => {
//   const d = new Date();
//   d.setTime(d.getTime() + 14 * 24 * 60 * 60 * 1000); //14 days

//   return `${
//     process.env.NEXT_PUBLIC_AUTH_NAME
//   }=${token}; expires=${d.toUTCString()}; path=/; SameSite=Strict`;
// };

// const setCookie = (token: string) => {
//   document.cookie = genSetCookie(token);
// };

// const deleteCookie = () => {
//   document.cookie = `${process.env.NEXT_PUBLIC_AUTH_NAME}=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/`;
// };
