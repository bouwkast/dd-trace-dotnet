// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2022 Datadog, Inc.

#pragma once

#include "RawSample.h"
#include "Sample.h"

class GCBaseRawSample : public RawSample
{
public:
    // This base class is in charge of storing garbage collection number and generation as labels
    // and fill up the callstack based on generation.
    // The default value is the Duration field; derived class could override by implementing GetValue()
    inline void OnTransform(Sample& sample, uint32_t valueOffset) const override
    {
        uint32_t durationIndex = valueOffset;

        sample.AddValue(GetValue(), durationIndex);

        sample.AddLabel(Label(Sample::GarbageCollectionNumberLabel, std::to_string(Number)));
        AddGenerationLabel(sample, Generation);

        BuildCallStack(sample, Generation);

        // let child classes transform additional fields if needed
        DoAdditionalTransform(sample, valueOffset);
    }

    // Each derived class provides the duration to store as the value for this sample
    // By default, use the Duration field
    virtual int64_t GetValue() const
    {
        return Duration;
    }

    // Derived classes are expected to set the event type + any additional field as label
    virtual void DoAdditionalTransform(Sample& sample, uint32_t valueOffset) const = 0;


public:
    int32_t Number;
    uint32_t Generation;
    int64_t Duration;

private:
    inline static void AddGenerationLabel(Sample& sample, uint32_t generation)
    {
        switch (generation)
        {
            case 0:
                sample.AddLabel(Label(Sample::GarbageCollectionGenerationLabel, Gen0Value));
                break;

            case 1:
                sample.AddLabel(Label(Sample::GarbageCollectionGenerationLabel, Gen1Value));
                break;

            case 2:
                sample.AddLabel(Label(Sample::GarbageCollectionGenerationLabel, Gen2Value));
                break;

            default: // this should never happen (only gen0, gen1 or gen2 collections)
                sample.AddLabel(Label(Sample::GarbageCollectionGenerationLabel, std::to_string(generation)));
                break;
        }
    }

    inline static void BuildCallStack(Sample& sample, uint32_t generation)
    {
        // add same root frame
        sample.AddFrame(EmptyModule, RootFrame);

        // add generation based frame
        switch (generation)
        {
            case 0:
                sample.AddFrame(EmptyModule, Gen0Frame);
                break;

            case 1:
                sample.AddFrame(EmptyModule, Gen1Frame);
                break;

            case 2:
                sample.AddFrame(EmptyModule, Gen2Frame);
                break;

            default:
                sample.AddFrame(EmptyModule, UnknownGenerationFrame);
                break;
        }
    }

private:
    static const inline std::string Gen0Value = "0";
    static const inline std::string Gen1Value = "1";
    static const inline std::string Gen2Value = "2";

    // each Stop the World garbage collection will share the same root frame and the second one will show the collected generation
    static constexpr inline std::string_view EmptyModule = "CLR";
    static constexpr inline std::string_view RootFrame = "|lm: |ns: |ct: |fn:Garbage Collector";
    static constexpr inline std::string_view Gen0Frame = "|lm: |ns: |ct: |fn:gen0";
    static constexpr inline std::string_view Gen1Frame = "|lm: |ns: |ct: |fn:gen1";
    static constexpr inline std::string_view Gen2Frame = "|lm: |ns: |ct: |fn:gen2";
    static constexpr inline std::string_view UnknownGenerationFrame = "|lm: |ns: |ct: |fn:unknown";
};