import { JSX, LazyExoticComponent } from "react";

export type RouteConfig = {
  path: string;
  element: LazyExoticComponent<() => JSX.Element>;
  private: boolean;
};