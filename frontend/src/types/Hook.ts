export type ParamHook<T, V> = (params: V, otherParams?: unknown[]) => T;
export type SimpleHook<T> = () => T;

export type Hook<T, V = unknown> = ParamHook<T, never> | ParamHook<T, V>;
