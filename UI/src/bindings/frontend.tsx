import engine from "cohtml/cohtml";
import { bindLocalValue } from "cs2/api";

export const ClosePanel = () => {
  editorPanelBinding.update(false);
  engine.trigger("audio.playSound", "select-item", 1);
};

export const editorPanelBinding = bindLocalValue(true);

export const visibleBindings = [editorPanelBinding];

export enum PanelIndex {
  Editor = 0,
}

export const togglePanel = (indexToToggle: number) => {
  const currentlyOpen = visibleBindings[indexToToggle].value;

  visibleBindings.forEach((binding, i) => {
    binding.update(i === indexToToggle ? !currentlyOpen : false);
  });
};
