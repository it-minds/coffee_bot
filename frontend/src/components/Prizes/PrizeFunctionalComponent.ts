import { Component, FunctionComponent, ReactNode } from "react";

// interface FunctionComponent<P = {}> {
//   (props: PropsWithChildren<P>, context?: any): ReactElement<any, any> | null;
//   propTypes?: WeakValidationMap<P> | undefined;
//   contextTypes?: ValidationMap<any> | undefined;
//   defaultProps?: Partial<P> | undefined;
//   displayName?: string | undefined;
// }

export interface PriceFunctionalComponent<T = Record<string, never>> extends FunctionComponent<T> {
  _type: "PriceFunctionalComponent";
}

export type PFC<T = unknown> = PriceFunctionalComponent<T>;

export function isPFC(component: ReactNode | Component | PFC): component is PFC {
  return (component as PFC)._type === "PriceFunctionalComponent";
}
