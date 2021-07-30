export const IsomorphicAbortController: typeof AbortController =
  typeof window === "undefined" ? require("abort-controller") : window.AbortController;

export default IsomorphicAbortController;

//! Note from Node16 a defauult implementation is avilable. https://nodejs.org/api/globals.html#globals_class_abortcontroller
