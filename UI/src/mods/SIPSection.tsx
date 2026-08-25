import { SelectedInfoSectionBase } from "cs2/bindings";
import { MeshStatData, MeshStats } from "types/MeshStats";
import mod from "mod.json";
import * as l10n from "cs2/l10n";
import { Unit } from "cs2/l10n";
import {
  FindTranslation,
  initTranslationService,
  localizedFloat2Number,
} from "shared/lang";
import { Icon, PanelFoldout, PanelSectionRow, Tooltip } from "cs2/ui";
import { GetSectionOpen, SetSectionOpen } from "shared/section";
import { FOCUS_AUTO } from "cs2/input";

import { Divider } from "shared/vanilla";

import sipSectionStyle from "./SIPSection.module.scss";
import commonStyle from "styles/common.module.scss";
import { errorIcon, warningIcon } from "shared/icons";
import { infoRowModule } from "shared/style";
import { SIP_MSV_NAME } from "index";
import { Options, View } from "types/Options";

interface SIP_MSV extends SelectedInfoSectionBase {
  prefabName: string;
  hasMesh: boolean;
  meshStatsInfo: MeshStats[];
  options: Options;
}

interface DataProps {
  meshStat: MeshStats;
  noLOD: boolean;
  smallAsset: boolean;
  options: Options;
}

let meshStatsLocal: MeshStats[] = [];

let SurfaceAreaText: string = "";
let VertexCountText: string = "";
let TrisCountText: string = "";
let TrisDensityText: string = "";
let TrisReductionText: string = "";
let VolumeText: string = "";

let SurfaceAreaTooltip: string = "";
let VertexCountTooltip: string = "";
let TrisCountTooltip: string = "";
let TrisDensityTooltip: string = "";
let TrisReductionTooltip: string = "";
let VolumeTooltip: string = "";

let NoLOD: string = "";
let NoLODButSmall: string = "";
let MinimalLODReduction: string = "";
let HighTrisDensity: string = "";
let ComplexSmallMesh: string = "";
let HeavyMeshNoLOD: string = "";
let HeavyMeshNoLODCheeky: string = "";
let LODHeavier: string = "";
let LODIdentical: string = "";
let AllLODsIdentical: string = "";

const cheekyEyes: string = "assetdb://global/ef193229fbf684ef095c2b867dcdd100";
const cheekySerious: string =
  "assetdb://global/0c68b3c93afc322058378d412fb1d907";

const BuildTabular = (props: DataProps): any => {
  const fields: (keyof MeshStatData)[] = [
    "TrisCount",
    "VertexCount",
    "TrisReduction",
    "TrisDensity",
    "SurfaceArea",
    "Volume",
  ];

  const totalGroups = props.meshStat.LODCount + 1;
  const data = props.meshStat.MeshStatData;

  const allTrisEqual: boolean =
    data.length > 1 &&
    data.every(rp => rp.TrisCount >= 500 && rp.TrisCount === data[0].TrisCount);

  const pairWarnings = Array.from({ length: totalGroups }, (_, groupIndex) => {
    if (groupIndex == 0) return null;
    const rp = data[groupIndex];
    const prevRp = data[groupIndex - 1];
    if (!rp || !prevRp) return null;

    const lodIsHeavier = rp.TrisCount > prevRp.TrisCount;
    const lodIsIdentical =
      rp.TrisCount >= 500 && !allTrisEqual && rp.TrisCount === prevRp.TrisCount;

    if (!lodIsHeavier && !lodIsIdentical) return null;
    return { groupIndex, lodIsHeavier, lodIsIdentical };
  }).filter(Boolean) as {
    groupIndex: number;
    lodIsHeavier: boolean;
    lodIsIdentical: boolean;
  }[];

  return (
    <>
      {allTrisEqual && (
        <PanelSectionRow
          disableFocus={true}
          center={
            <>
              <Icon
                src={errorIcon}
                className={`${infoRowModule.icon} ${infoRowModule.link}`}
              />
              {AllLODsIdentical}
              {props.options.CheekyMode && (
                <Icon
                  src={cheekySerious}
                  className={`${infoRowModule.icon} ${infoRowModule.link}`}
                />
              )}
            </>
          }
        />
      )}

      {!allTrisEqual &&
        pairWarnings.map(w => (
          <PanelSectionRow
            key={w.groupIndex}
            disableFocus={true}
            center={
              <>
                <Icon
                  src={errorIcon}
                  className={`${infoRowModule.icon} ${infoRowModule.link}`}
                />
                {`${(w.lodIsHeavier ? LODHeavier : LODIdentical).replace("LOD", `LOD${w.groupIndex}`)}`}
                {props.options.CheekyMode && (
                  <Icon
                    src={cheekySerious}
                    className={`${infoRowModule.icon} ${infoRowModule.link}`}
                  />
                )}
              </>
            }
          />
        ))}
      <PanelSectionRow
        right={
          <div
            className={`${sipSectionStyle.RowFlex} ${totalGroups == 1 ? commonStyle.JustifySpaceAround : ""}`}
          >
            <div>
              <div>{`...`}</div>
              {fields.map(field => (
                <div key={field}>{FindTranslation(`${field}Text`)}</div>
              ))}
            </div>
            {Array.from({ length: totalGroups }, (_, groupIndex) => (
              <div key={groupIndex}>
                <div>{groupIndex == 0 ? "Main" : `LOD${groupIndex}`}</div>
                {fields.map(field => {
                  return (
                    <div key={field}>
                      {field == "TrisReduction" &&
                      props.meshStat.MeshStatData[groupIndex]?.[field] == -1
                        ? "-"
                        : localizedFloat2Number(
                            props.meshStat.MeshStatData[groupIndex]?.[
                              field
                            ] as number,
                          )}
                    </div>
                  );
                })}
              </div>
            ))}
          </div>
        }
      />
    </>
  );
};

const BuildLonger = (props: DataProps): any => {
  const stat: MeshStats = props.meshStat;
  const smallAsset = props.smallAsset;
  const options = props.options;
  let prev: number = 0;
  let lodnum: number = 0;

  const simplified: boolean = options.ViewChooser == View.Simplified;

  const allTrisEqual: boolean =
    stat.MeshStatData.length > 1 &&
    stat.MeshStatData.every(
      rp => rp.TrisCount === stat.MeshStatData[0].TrisCount,
    );

  return (
    <>
      {allTrisEqual && (
        <PanelSectionRow
          disableFocus={true}
          center={
            <>
              <Icon
                src={errorIcon}
                className={`${infoRowModule.icon} ${infoRowModule.link}`}
              />
              {AllLODsIdentical}
              {props.options.CheekyMode && (
                <Icon
                  src={cheekySerious}
                  className={`${infoRowModule.icon} ${infoRowModule.link}`}
                />
              )}
            </>
          }
        />
      )}
      {stat.MeshStatData.map(rp => {
        const trisReductionValue: number = (100 * (prev - rp.TrisCount)) / prev;
        const trisReductionString: any =
          prev == 0 ? "-" : localizedFloat2Number(trisReductionValue);
        const lodIsHeavier: boolean = lodnum > 0 && rp.TrisCount > prev;
        const lodIsIdentical: boolean =
          !allTrisEqual && lodnum > 0 && rp.TrisCount === prev;
        const minimalLODReduction: boolean =
          lodnum == 0 ||
          smallAsset ||
          lodIsHeavier ||
          lodIsIdentical ||
          (lodnum == 2 && rp.TrisCount < 500)
            ? false
            : lodnum == 1
              ? trisReductionValue < options.LOD1Threshold
              : trisReductionValue < options.LOD2Threshold;
        const highTrisDensity: boolean =
          rp.TrisDensity > (options.TrisThreshold ?? 0) / 10;
        const complexSmallMesh = rp.Volume < 0.05 && highTrisDensity;
        prev = rp.TrisCount;
        lodnum++;
        return (
          <>
            <PanelSectionRow disableFocus={true} center={rp.Name} />
            <PanelSectionRow
              disableFocus={true}
              center={
                <div
                  className={`${sipSectionStyle.RowFlex} ${!simplified && commonStyle.JustifySpaceAround}`}
                >
                  <div>
                    <Tooltip tooltip={TrisCountTooltip}>
                      <div className={commonStyle.MarginBottom5}>
                        <span className={commonStyle.Font90}>
                          {TrisCountText}
                        </span>
                        <br />
                        {localizedFloat2Number(rp.TrisCount)}
                      </div>
                    </Tooltip>
                    {!simplified && (
                      <Tooltip tooltip={TrisDensityTooltip}>
                        <div>
                          <span className={commonStyle.Font90}>
                            {TrisDensityText}
                          </span>
                          <br />
                          {localizedFloat2Number(rp.TrisDensity)}
                        </div>
                      </Tooltip>
                    )}
                  </div>
                  <div>
                    <Tooltip tooltip={TrisReductionTooltip}>
                      <div className={commonStyle.MarginBottom5}>
                        <span className={commonStyle.Font90}>
                          {minimalLODReduction && (
                            <Tooltip tooltip={MinimalLODReduction}>
                              <img
                                className={infoRowModule.icon}
                                src={warningIcon}
                              />
                            </Tooltip>
                          )}
                          {TrisReductionText}
                        </span>
                        <br />
                        {trisReductionString}
                      </div>
                    </Tooltip>
                    {!simplified && (
                      <Tooltip tooltip={SurfaceAreaTooltip}>
                        <div>
                          <span className={commonStyle.Font90}>
                            {SurfaceAreaText}
                          </span>
                          <br />
                          {localizedFloat2Number(rp.SurfaceArea)}
                        </div>
                      </Tooltip>
                    )}
                  </div>
                  {!simplified && (
                    <div>
                      <Tooltip tooltip={VertexCountTooltip}>
                        <div className={commonStyle.MarginBottom5}>
                          <span className={commonStyle.Font90}>
                            {VertexCountText}
                          </span>
                          <br />
                          {localizedFloat2Number(rp.VertexCount)}
                        </div>
                      </Tooltip>
                      <Tooltip tooltip={VolumeTooltip}>
                        <div>
                          <span className={commonStyle.Font90}>
                            {VolumeText}
                          </span>
                          <br />
                          {localizedFloat2Number(rp.Volume)}
                        </div>
                      </Tooltip>
                    </div>
                  )}
                </div>
              }
            />
            {highTrisDensity && (
              <PanelSectionRow
                disableFocus={true}
                center={
                  <>
                    <Icon
                      src={warningIcon}
                      className={`${infoRowModule.icon} ${infoRowModule.link}`}
                    />
                    {HighTrisDensity}
                    {props.options.CheekyMode && (
                      <Icon
                        src={cheekyEyes}
                        className={`${infoRowModule.icon} ${infoRowModule.link}`}
                      />
                    )}
                  </>
                }
              />
            )}
            {complexSmallMesh && (
              <PanelSectionRow
                disableFocus={true}
                center={
                  <>
                    <Icon
                      src={warningIcon}
                      className={`${infoRowModule.icon} ${infoRowModule.link}`}
                    />
                    {ComplexSmallMesh}
                    {props.options.CheekyMode && (
                      <Icon
                        src={cheekyEyes}
                        className={`${infoRowModule.icon} ${infoRowModule.link}`}
                      />
                    )}
                  </>
                }
              />
            )}
            {lodIsHeavier && (
              <PanelSectionRow
                disableFocus={true}
                center={
                  <>
                    <Icon
                      src={errorIcon}
                      className={`${infoRowModule.icon} ${infoRowModule.link}`}
                    />
                    {LODHeavier}
                    {props.options.CheekyMode && (
                      <Icon
                        src={cheekySerious}
                        className={`${infoRowModule.icon} ${infoRowModule.link}`}
                      />
                    )}
                  </>
                }
              />
            )}
            {lodIsIdentical && (
              <PanelSectionRow
                disableFocus={true}
                center={
                  <>
                    <Icon
                      src={errorIcon}
                      className={`${infoRowModule.icon} ${infoRowModule.link}`}
                    />
                    {LODIdentical}
                    {props.options.CheekyMode && (
                      <Icon
                        src={cheekySerious}
                        className={`${infoRowModule.icon} ${infoRowModule.link}`}
                      />
                    )}
                  </>
                }
              />
            )}
          </>
        );
      })}
    </>
  );
};

export const SIP_MSV = (componentList: any): any => {
  componentList[SIP_MSV_NAME] = (props: SIP_MSV) => {
    const { translate } = l10n.useLocalization();
    initTranslationService(translate);

    if (props.hasMesh) {
      meshStatsLocal = props.meshStatsInfo;
    }
    const modNameText = mod.name;

    SurfaceAreaText = FindTranslation("SurfaceAreaText");
    VertexCountText = FindTranslation("VertexCountText");
    TrisCountText = FindTranslation("TrisCountText");
    TrisDensityText = FindTranslation("TrisDensityText");
    TrisReductionText = FindTranslation("TrisReductionText");
    VolumeText = FindTranslation("VolumeText");

    SurfaceAreaTooltip = FindTranslation("SurfaceAreaTooltip");
    VertexCountTooltip = FindTranslation("VertexCountTooltip");
    TrisCountTooltip = FindTranslation("TrisCountTooltip");
    TrisDensityTooltip = FindTranslation("TrisDensityTooltip");
    TrisReductionTooltip = FindTranslation("TrisReductionTooltip");
    VolumeTooltip = FindTranslation("VolumeTooltip");

    NoLOD = FindTranslation("NoLOD");
    NoLODButSmall = FindTranslation("NoLODButSmall");
    MinimalLODReduction = FindTranslation("MinimalLODReduction");
    HighTrisDensity = FindTranslation("HighTrisDensity");
    ComplexSmallMesh = FindTranslation("ComplexSmallMesh");
    HeavyMeshNoLOD = FindTranslation("HeavyMeshNoLOD");
    HeavyMeshNoLODCheeky = FindTranslation("HeavyMeshNoLODCheeky");
    LODHeavier = FindTranslation("LODHeavier");
    LODIdentical = FindTranslation("LODIdentical");
    AllLODsIdentical = FindTranslation("AllLODsIdentical");

    return (
      <PanelFoldout
        key={`msv-main`}
        initialExpanded={GetSectionOpen("msv-main", true)}
        onToggleExpanded={v => SetSectionOpen("msv-main", v)}
        expandFromContent={true}
        focusKey={FOCUS_AUTO}
        header={
          <PanelSectionRow
            uppercase={true}
            disableFocus={true}
            left={modNameText}
          />
        }
      >
        <PanelSectionRow
          disableFocus={true}
          left={"Prefab"}
          right={props.prefabName}
        />
        {meshStatsLocal.map((stat, i1) => {
          const noLOD: boolean =
            stat.LODCount <= 0 && stat.MeshStatData[0].TrisCount > 1000;

          const smallAsset: boolean =
            stat.MeshStatData[0].Volume <= props.options.VolumeThreshold;

          const heavyTris: boolean = stat.MeshStatData[0].TrisCount > 100_000;

          const title: string =
            props.options.ViewChooser == View.Tabular
              ? `Mesh #${i1 + 1} of ${meshStatsLocal.length}: ${stat.MeshStatData[0].Name}`
              : `Mesh #${i1 + 1} of ${meshStatsLocal.length}`;

          return (
            <>
              {i1 > 0 ? <Divider noMargin={2} /> : null}
              {meshStatsLocal.length > 1 && (
                <PanelSectionRow disableFocus={true} center={title} />
              )}

              {noLOD &&
                (smallAsset ? (
                  <PanelSectionRow disableFocus={true} center={NoLODButSmall} />
                ) : heavyTris ? (
                  <PanelSectionRow
                    disableFocus={true}
                    center={
                      <>
                        <Icon
                          src={errorIcon}
                          className={`${infoRowModule.icon} ${infoRowModule.link}`}
                        />
                        {props.options.CheekyMode
                          ? HeavyMeshNoLODCheeky
                          : HeavyMeshNoLOD}
                        {props.options.CheekyMode && (
                          <Icon
                            src={cheekySerious}
                            className={`${infoRowModule.icon} ${infoRowModule.link}`}
                          />
                        )}
                      </>
                    }
                  />
                ) : (
                  <PanelSectionRow
                    disableFocus={true}
                    center={
                      <>
                        <Icon
                          src={warningIcon}
                          className={`${infoRowModule.icon} ${infoRowModule.link}`}
                        />
                        {NoLOD}
                        {props.options.CheekyMode && (
                          <Icon
                            src={cheekyEyes}
                            className={`${infoRowModule.icon} ${infoRowModule.link}`}
                          />
                        )}
                      </>
                    }
                  />
                ))}

              {props.options.ViewChooser == View.Tabular ? (
                <BuildTabular
                  meshStat={stat}
                  smallAsset={smallAsset}
                  noLOD={noLOD}
                  options={props.options}
                />
              ) : (
                <BuildLonger
                  meshStat={stat}
                  smallAsset={smallAsset}
                  noLOD={noLOD}
                  options={props.options}
                />
              )}
            </>
          );
        })}
      </PanelFoldout>
    );
  };
  return componentList as any;
};
